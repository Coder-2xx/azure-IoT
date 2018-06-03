import { DeviceManagerRoutingModule } from './device-manager.routing.module';

describe('DeviceManager.RoutingModule', () => {
  let deviceManagerRoutingModule: DeviceManagerRoutingModule;

  beforeEach(() => {
    deviceManagerRoutingModule = new DeviceManagerRoutingModule();
  });

  it('should create an instance', () => {
    expect(deviceManagerRoutingModule).toBeTruthy();
  });
});
