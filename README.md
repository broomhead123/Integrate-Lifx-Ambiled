Integrates LIFX web api with the serial AmbiLED interface

Needs API key from LIFX for access to bulb.

Currently updates AmbiLED to use the color values from LIFX web api

TODO:
  Pull out constants (api key, light name) and other control out to the UI
  
  Implement lan protocol for LIFX communication (faster than through API)
  
  Add in ability to calibrate ambiled to better match lifx
