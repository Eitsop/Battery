# Battery
Provides both a managed and unmanaged proxy to the system batteries.

## Gmcu.Proxy.Io.Battery

This provides a simple DLL that provides methods to query the state of the system battery and power state.

## Gmcu.Managed.Io.Battery

This uses the Gmcu.Proxy.Io.Battery class and provides a managed class to work with.

Basically you'll want to:

~~~
using Gmcu.Managed.Io.Battery;

var powerState = new PowerState();
~~~

I've commented the code, so navigate about using intellisense.
You might find something useful there.
