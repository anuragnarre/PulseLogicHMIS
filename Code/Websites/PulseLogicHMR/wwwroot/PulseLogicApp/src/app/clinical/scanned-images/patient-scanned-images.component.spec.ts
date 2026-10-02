import { PatientScannedImagesComponent } from './patient-scanned-images.component';

describe('PatientScannedImagesComponent', () => {
  let component: PatientScannedImagesComponent;
  let mockSecurityService, mockPatientService, mockVisitService, mockMsgBoxSrv, mockHistoServ, mockDoctorsBLService, mockLightbox, mockSanitizer;

  beforeEach(() => {
    mockSecurityService = { GetLoggedInUser: () => ({ EmployeeId: 1 }) };
    mockPatientService = {};
    mockVisitService = { globalVisit: { PatientId: 1 } };
    mockMsgBoxSrv = { showMessage: jasmine.createSpy('showMessage') };
    mockHistoServ = { GetUploadedPatientImages: () => ({ subscribe: () => {} }) };
    mockDoctorsBLService = { GetDepartMent: () => ({ subscribe: () => {} }) };
    mockLightbox = {};
    mockSanitizer = {};

    component = new PatientScannedImagesComponent(
      mockSecurityService as any,
      mockPatientService as any,
      mockVisitService as any,
      mockMsgBoxSrv as any,
      mockHistoServ as any,
      mockDoctorsBLService as any,
      mockLightbox as any,
      mockSanitizer as any
    );
  });

  describe('MakeImgAlbum', () => {
    it('should catch error when uploadedImagesList is null', () => {
      // Intentionally cause an error in MakeImgAlbum by setting uploadedImagesList to null
      component.uploadedImagesList = null as any;

      // Spy on ShowCatchErrMessage if we need to check if error was logged or caught
      // Currently, the catch block is empty in the original code, so we just expect no throw
      expect(() => component.MakeImgAlbum()).not.toThrow();

      // Initially set arrays
      expect(component.album).toEqual([]);
      expect(component.pdfAlbum).toEqual([]);
    });

    it('should catch error when uploadedImagesList is undefined', () => {
      component.uploadedImagesList = undefined as any;
      expect(() => component.MakeImgAlbum()).not.toThrow();
      expect(component.album).toEqual([]);
      expect(component.pdfAlbum).toEqual([]);
    });
  });
});
