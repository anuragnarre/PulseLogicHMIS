## 2025-02-12 - N+1 Issue in LabController LabSpecimen loop
**Learning:** In `LabController.cs`, `EditLabTestResult` and `AddRequsitionFromBilling` had an N+1 query issue where it iterated through `labSpecimenList`, queried the db by RequisitionId, updated, and saved changes *within* the loop.
**Action:** Replaced the loop with a bulk fetch (Where In), an in-memory matching, and a single SaveChanges call. This eliminates the N+1 problem. Also replaced `int` cast with `long` cast where RequisitionId is long nullable.
