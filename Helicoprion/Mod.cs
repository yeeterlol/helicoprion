using GDWeave;

namespace Helicoprion;

public class Mod : IMod {
    public Config Config;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        modInterface.Logger.Information("Helicoprion 0.0.1 loaded");
        modInterface.RegisterScriptMod(new AnticheatPlayer());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
