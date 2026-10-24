using DiGi.Core.Classes;
using DiGi.GIS.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Handles the process of posting orthodata retrieved from the database.
    /// </summary>
    public class OrtoDatasFromDatabasePostTask : OrtoDatasPostTask
    {
        /// <summary>
        /// Handles the process of posting orthodata retrieved from the database.
        /// </summary>
        /// <param name="GISWebAPIManager">The manager responsible for handling GIS PostgreSQL Web API communications.</param>
        public OrtoDatasFromDatabasePostTask(GISWebAPIManager GISWebAPIManager)
            : base(GISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the number of items to claim per batch from the update queue. Defaults to 5.
        /// </summary>
        public int Count { get; set; } = 5;

        /// <summary>
        /// Gets or sets the options used for retrieving 2D building orthophoto data.
        /// </summary>
        public OrtoDatasBuilding2DOptions? OrtoDatasBuilding2DOptions { get; set; } = new();

        /// <summary>
        /// Gets or sets how many failed batches in a row stop the task. Defaults to 3; values below 1 are treated as 1.
        /// <para>A batch fails when a request times out or the connection drops, when Building2Ds cannot be fetched, or when the upload or the acknowledge is not accepted. Its references stay claimed and return to the queue when their lease expires, so the run moves on to the next batch. A batch that stores OrtoDatas resets the count. Reaching the limit ends the task with a <see cref="TimeoutException"/> naming the county and the last failure, so a dead server still stops the run.</para>
        /// <para>No host exposes this setting, so the default is the production value.</para>
        /// </summary>
        public int MaxConsecutiveFailureCount { get; set; } = 3;

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoDatasFromDatabasePostTask), nameof(ExecuteAsync));

            if (GISWebAPIManager is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "GISWebAPIManager cannot be null");
                return false;
            }

            HttpClient? httpClient_OrtoDatas = GISWebAPIManager.CreateHttpClient<OrtoDatasController>(nameof(OrtoDatasController.NextBuilding2DReferencesAsync), out string? path_OrtoDatas);
            if (httpClient_OrtoDatas is null || string.IsNullOrWhiteSpace(path_OrtoDatas))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "OrtoDatas HttpClient could not be created");
                return false;
            }

            string requestUri_OrtoDatas = new UrlBuilder(path_OrtoDatas).AddParameter("count", Count > 0 ? Count : 5).ToString();

            HttpClient? httpClient_Building2D = GISWebAPIManager.CreateHttpClient<Building2DController>(nameof(Building2DController.GetItemsByBuilding2DReferencesAsync), out string? path_Building2D);
            if (httpClient_Building2D is null || string.IsNullOrWhiteSpace(path_Building2D))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2D HttpClient could not be created");
                return false;
            }

            HttpClient? httpClient_Geoportal = Create.HttpClient_Geoportal(GISWebAPIManager);
            if (httpClient_Geoportal is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Geoportal HttpClient could not be created");
                return false;
            }

            HttpClient? httpClient_OrtoDatas_Acknowledge = GISWebAPIManager.CreateHttpClient<OrtoDatasController>(nameof(OrtoDatasController.AcknowledgeBuilding2DReferencesAsync), out string? path_OrtoDatas_Acknowledge);

            string requestUri_Building2D = new UrlBuilder(path_Building2D).ToString();

            PostOptions postOptions = new() { RequestResult = true };

            // Every polygon part of each county, keyed by part id. A queue entry names the part its building was under when it was
            // queued, and a building can since have moved to a sibling part - the 86 196 sibling duplicates removed on 2026-08-14 left
            // whole parts queued with no building_2d row (58 648 entries under 25886, tomaszowski 1016, whose buildings are under
            // 25887). Sending every part makes the upload file each entry under the part its building_2d row is in; a single id is
            // written verbatim. Without the list a part stands for itself, which is what a single-part county is anyway.
            Dictionary<int, List<int>> countyIds_Parts = [];
            HttpClient? httpClient_AdministrativeAreal2D = GISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync), out string? path_AdministrativeAreal2D);
            if (httpClient_AdministrativeAreal2D is not null && !string.IsNullOrWhiteSpace(path_AdministrativeAreal2D))
            {
                try
                {
                    // The administrativearealtype parameter is not nullable-safe on omission - always sent.
                    string requestUri_AdministrativeAreal2D = new UrlBuilder(path_AdministrativeAreal2D).AddParameter("administrativearealtype", (int)PostgreSQL.Enums.AdministrativeArealType.County).ToString();
                    PostResponse<List<AdministrativeAreal2DReference>?> postResponse_AdministrativeAreal2DReferences = await DiGi.WebAPI.Query.GetAsync<List<AdministrativeAreal2DReference>>(httpClient_AdministrativeAreal2D, requestUri_AdministrativeAreal2D, postOptions);
                    if (postResponse_AdministrativeAreal2DReferences?.Result is List<AdministrativeAreal2DReference> administrativeAreal2DReferences)
                    {
                        foreach (IGrouping<string, AdministrativeAreal2DReference> grouping in administrativeAreal2DReferences.Where(x => x is not null && !string.IsNullOrWhiteSpace(x.Code)).GroupBy(x => x.Code!))
                        {
                            List<int> countyIds_Code = [.. grouping.Select(x => x.Id).Distinct().OrderBy(x => x)];
                            foreach (int countyId_Part in countyIds_Code)
                            {
                                countyIds_Parts[countyId_Part] = countyIds_Code;
                            }
                        }
                    }
                }
                catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
                {
                    Serilog.Modify.Log(exception, "County parts could not be read; each queue entry is uploaded under the part it was queued under");
                }
            }

            List<int> CountyIds(int countyId)
            {
                return countyIds_Parts.TryGetValue(countyId, out List<int>? countyIds_Code) ? countyIds_Code : [countyId];
            }

            int maxConsecutiveFailureCount = MaxConsecutiveFailureCount < 1 ? 1 : MaxConsecutiveFailureCount;
            int failureCount = 0;

            // A request timeout surfaces as an OperationCanceledException while the task's own token is not cancelled -
            // PostAsync bounds each attempt with its own token - so the token tells a user cancel from a timeout.
            // A failure is counted, not thrown, until the limit is reached; the references it held stay claimed.
            void RegisterFailure(string reason, Exception? exception)
            {
                failureCount++;
                if (failureCount >= maxConsecutiveFailureCount)
                {
                    throw new TimeoutException(string.Format("OrtoDatas upload stopped after {0} consecutive failed batches. Last failure: {1}{2}", failureCount, reason, exception is null ? string.Empty : string.Format(" ({0})", exception.Message)), exception);
                }
            }

            // TODO [PostAsyncStatusCode]: PostAsync reports a non-transient response only as a plain Exception whose message names the
            // status code, so a 404 is recognised by that message. Remove once DiGi.WebAPI.Modify.PostAsync exposes the status code of a
            // failed response, and match on that instead.
            static bool IsNotFound(Exception exception)
            {
                return exception.GetType() == typeof(Exception) && exception.Message.StartsWith(string.Format("Server returned {0}.", System.Net.HttpStatusCode.NotFound), StringComparison.Ordinal);
            }

            // A factory rather than a single-use HttpContent - the body is rebuilt per attempt, so a dropped pooled connection is retried
            // instead of skipping the batch. The endpoint answers 404 when none of the references is found, which is an empty result,
            // not a fault: answered as an empty list. Null means the fetch did not succeed; any other failure is thrown.
            async Task<List<GIS.Classes.Building2D>?> Building2DsAsync(List<Building2DReference> building2DReferences_Fetch)
            {
                try
                {
                    PostResponse<List<GIS.Classes.Building2D>?> postResponse_Building2Ds = await DiGi.WebAPI.Modify.PostAsync<List<GIS.Classes.Building2D>>(httpClient_Building2D, requestUri_Building2D, async () => await Create.HttpContent(building2DReferences_Fetch, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("HttpContent for Building2D references could not be created"), postOptions);
                    if (postResponse_Building2Ds is null || !postResponse_Building2Ds.Succeeded)
                    {
                        return null;
                    }

                    return postResponse_Building2Ds.Result ?? [];
                }
                catch (Exception exception) when (IsNotFound(exception))
                {
                    return [];
                }
            }

            async Task<PostResponse<List<Building2DReference>?>> ClaimAsync()
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        return await DiGi.WebAPI.Modify.PostAsync<List<Building2DReference>>(httpClient_OrtoDatas, requestUri_OrtoDatas, (HttpContent?)null, postOptions);
                    }
                    catch (OperationCanceledException operationCanceledException) when (!cancellationToken.IsCancellationRequested)
                    {
                        Serilog.Modify.Log(operationCanceledException, "Claiming Building2DReferences timed out after {Delay}s", postOptions.Delay.TotalSeconds);
                        RegisterFailure(string.Format("claiming Building2DReferences timed out after {0}s", postOptions.Delay.TotalSeconds), operationCanceledException);
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        Serilog.Modify.Log(httpRequestException, "HTTP error while claiming Building2DReferences");
                        RegisterFailure("HTTP error while claiming Building2DReferences", httpRequestException);
                    }
                }
            }

            PostResponse<List<Building2DReference>?> postResponse_Building2DReferences = await ClaimAsync();

            if (postResponse_Building2DReferences is null || !postResponse_Building2DReferences.Succeeded)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DReferences could not be retrieved from the queue");
                return false;
            }

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);
            OrtoDatasBuilding2DOptions ortoDatasBuilding2DOptions = OrtoDatasBuilding2DOptions ?? new();

            while (postResponse_Building2DReferences is not null && postResponse_Building2DReferences.Succeeded && postResponse_Building2DReferences.Result is List<Building2DReference> building2DReferences && building2DReferences.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                while (building2DReferences.Count > 0)
                {
                    int? countyId = building2DReferences[0].CountyId;

                    Core.Query.Filter(building2DReferences, x => x?.CountyId == countyId, out List<Building2DReference>? building2DReference_In, out List<Building2DReference>? building2DReferences_Out);
                    building2DReferences = building2DReferences_Out ?? [];

                    if (building2DReference_In != null && building2DReference_In.Count != 0 && countyId is not null && countyId.HasValue)
                    {
                        List<Building2DReference> building2DReferences_Claimed = building2DReference_In;

                        try
                        {
                            // Fetched under the part the entries were queued under first: that read is pruned to one partition.
                            List<GIS.Classes.Building2D>? building2Ds = await Building2DsAsync(building2DReferences_Claimed);
                            if (building2Ds is null)
                            {
                                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2Ds could not be fetched for {Count} references in county {CountyId}", building2DReferences_Claimed.Count, countyId.Value);
                                RegisterFailure(string.Format("Building2Ds could not be fetched for county {0}", countyId.Value), null);
                                continue;
                            }

                            // An entry whose building is not under that part any more is looked up by its reference alone - the server
                            // reads every partition for a reference with no county - so a building that moved to a sibling part is still
                            // downloaded. Only the missing entries pay for that wider read.
                            HashSet<string> references_Found = [.. building2Ds.Select(x => x?.Reference).OfType<string>()];
                            List<Building2DReference> building2DReferences_Missing = [];
                            foreach (Building2DReference building2DReference in building2DReferences_Claimed)
                            {
                                if (building2DReference is not null && !string.IsNullOrWhiteSpace(building2DReference.Reference) && !references_Found.Contains(building2DReference.Reference))
                                {
                                    building2DReferences_Missing.Add(new Building2DReference(building2DReference) { CountyId = null });
                                }
                            }

                            if (building2DReferences_Missing.Count > 0)
                            {
                                List<GIS.Classes.Building2D>? building2Ds_ByReference = await Building2DsAsync(building2DReferences_Missing);
                                int count_ByReference = 0;
                                foreach (GIS.Classes.Building2D building2D_ByReference in building2Ds_ByReference ?? [])
                                {
                                    if (!string.IsNullOrWhiteSpace(building2D_ByReference?.Reference) && references_Found.Add(building2D_ByReference.Reference))
                                    {
                                        building2Ds.Add(building2D_ByReference);
                                        count_ByReference++;
                                    }
                                }

                                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Missing} of {Count} references were not under county part {CountyId}; {Found} found by reference alone", building2DReferences_Missing.Count, building2DReferences_Claimed.Count, countyId.Value, count_ByReference);
                            }

                            // Nothing found under any part: the entries outlived their building_2d rows. An empty batch, not a server
                            // fault, so it neither stops the run nor counts as a failure; the references stay claimed and return to the
                            // queue when their lease expires, and are retired after the queue's maximum number of attempts.
                            if (building2Ds.Count == 0)
                            {
                                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "No Building2Ds found for {Count} references in county {CountyId}; they stay claimed and return to the queue when their lease expires", building2DReferences_Claimed.Count, countyId.Value);
                                continue;
                            }

                            List<GIS.Classes.OrtoDatas> ortoDatasList = [];
                            foreach (GIS.Classes.Building2D building2D in building2Ds)
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                GIS.Classes.OrtoDatas? ortoDatas = await GIS.Create.OrtoDatas(httpClient_Geoportal, building2D, ortoDatasBuilding2DOptions.Years, ortoDatasBuilding2DOptions.Offset, ortoDatasBuilding2DOptions.Width, ortoDatasBuilding2DOptions.Reduce, squared: true);
                                if (ortoDatas is null)
                                {
                                    continue;
                                }

                                ortoDatasList.Add(ortoDatas);
                            }

                            if (ortoDatasList.Count > 0)
                            {
                                bool succeeded = await ExecuteAsync(ortoDatasList, CountyIds(countyId.Value), longProgressWrapper, cancellationToken);
                                if (succeeded)
                                {
                                    failureCount = 0;

                                    HashSet<string> references_Stored = [];
                                    foreach (GIS.Classes.OrtoDatas ortoDatas in ortoDatasList)
                                    {
                                        if (!string.IsNullOrWhiteSpace(ortoDatas?.Reference))
                                        {
                                            references_Stored.Add(ortoDatas.Reference);
                                        }
                                    }

                                    List<long> ids = [];
                                    foreach (Building2DReference building2DReference in building2DReferences_Claimed)
                                    {
                                        if (building2DReference is not null && building2DReference.Id > 0 && !string.IsNullOrWhiteSpace(building2DReference.Reference) && references_Stored.Contains(building2DReference.Reference))
                                        {
                                            ids.Add(building2DReference.Id);
                                        }
                                    }

                                    if (ids.Count > 0 && httpClient_OrtoDatas_Acknowledge is not null && !string.IsNullOrWhiteSpace(path_OrtoDatas_Acknowledge))
                                    {
                                        string json_Ack = System.Text.Json.JsonSerializer.Serialize(ids);

                                        // A factory rather than a single-use HttpContent, as for the Building2D fetch above, so a dropped
                                        // connection is retried. Stored but unacknowledged references are re-processed after lease expiry.
                                        PostResponse postResponse_Ack = await DiGi.WebAPI.Modify.PostAsync(httpClient_OrtoDatas_Acknowledge, path_OrtoDatas_Acknowledge, async () => await Create.HttpContent(json_Ack, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("HttpContent for acknowledge could not be created"), postOptions);
                                        if (postResponse_Ack is null || !postResponse_Ack.Succeeded)
                                        {
                                            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas stored but not acknowledged for {Count} references in county {CountyId}; they are re-processed after lease expiry", ids.Count, countyId.Value);
                                            RegisterFailure(string.Format("acknowledge was not accepted for county {0}", countyId.Value), null);
                                        }
                                    }

                                    if (ids.Count != building2DReferences_Claimed.Count)
                                    {
                                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas claimed but not stored: {Count}/{Total} references stay claimed and return to the queue when their lease expires", building2DReferences_Claimed.Count - ids.Count, building2DReferences_Claimed.Count);
                                    }
                                }
                                else
                                {
                                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas could not be updated for county {CountyId}. References remain claimed and will retry on lease expiry.", countyId.Value);
                                    RegisterFailure(string.Format("OrtoDatas update was not accepted for county {0}", countyId.Value), null);
                                }
                            }
                            else
                            {
                                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "No OrtoDatas imagery extracted for {Count} buildings in county {CountyId}", building2Ds.Count, countyId.Value);
                            }
                        }
                        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                        {
                            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} canceled", nameof(OrtoDatasFromDatabasePostTask), nameof(ExecuteAsync));
                            throw;
                        }
                        catch (OperationCanceledException operationCanceledException)
                        {
                            // A request timeout, not a user cancel: the references stay claimed and the lease expiry re-queues them.
                            Serilog.Modify.Log(operationCanceledException, "Request timed out during OrtoDatas processing in county {CountyId}; {Count} references stay claimed until lease expiry", countyId.Value, building2DReferences_Claimed.Count);
                            RegisterFailure(string.Format("request timed out in county {0} (limits: upload {1}s x {2} attempts, Building2D fetch and acknowledge {3}s x {4} attempts)", countyId.Value, SerializableObjectsPostOptions.Delay.TotalSeconds, SerializableObjectsPostOptions.RetryCount < 0 ? 1 : SerializableObjectsPostOptions.RetryCount + 1, postOptions.Delay.TotalSeconds, postOptions.RetryCount < 0 ? 1 : postOptions.RetryCount + 1), operationCanceledException);
                        }
                        catch (HttpRequestException httpRequestException)
                        {
                            Serilog.Modify.Log(httpRequestException, "HTTP error during OrtoDatas processing in county {CountyId}; {Count} references stay claimed until lease expiry", countyId.Value, building2DReferences_Claimed.Count);
                            RegisterFailure(string.Format("HTTP error in county {0}", countyId.Value), httpRequestException);
                        }
                        catch (Exception exception) when (exception is not TimeoutException)
                        {
                            Serilog.Modify.Log(exception, "Unexpected error during OrtoDatas processing in county {CountyId}", countyId.Value);
                            throw;
                        }
                    }
                }

                postResponse_Building2DReferences = await ClaimAsync();
            }

            Serilog.Modify.Log("{Type}:{Name} completed successfully", nameof(OrtoDatasFromDatabasePostTask), nameof(ExecuteAsync));
            return true;
        }
    }
}