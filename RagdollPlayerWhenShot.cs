using System;
using GTA;
using GTA.UI;

public class RagdollPlayerWhenShot : Script
{
    private readonly string version = "1.3.30.0";
    private int lastHealthAndArmor;
    private readonly Random random = new Random();
    private bool init = false;

    public RagdollPlayerWhenShot()
    {
        Tick += OnTick;
        lastHealthAndArmor = GetPlayerHealthAndArmorValue();
    }

    private void Initialize()
    {
        if (!init)
        {
            Screen.ShowHelpText($"RagdollPlayerWhenShot Script V{version}, By Fern Aerell.", 10000, true, false);
            Wait(10000);
            Screen.ShowHelpText($"Press ~INPUT_JUMP~ to wake up if you're stuck in ragdoll.", 10000, true, false);
            init = true;
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        Initialize();

        if (Game.Player.Character.IsAlive)
        {
            foreach (var ped in World.GetAllPeds())
            {
                if (ped.IsShooting && ped.CombatTarget != null && ped.CombatTarget.IsPlayer)
                {
                    int currentHealthAndArmor = GetPlayerHealthAndArmorValue();
                    if(currentHealthAndArmor < lastHealthAndArmor && !(Game.IsControlPressed(Control.Jump) && Game.Player.Character.IsRunningRagdollTask))
                    {
                        int minTime = random.Next(500, 1000);
                        float healthPercentage = (float)Game.Player.Character.Health / Game.Player.Character.MaxHealth;
                        int maxTime = (int)(minTime * (1 + (1 - healthPercentage)));
                        Game.Player.Character.SetToRagdoll(minTime, maxTime, RagdollType.Balance, true);
                    }
                    lastHealthAndArmor = currentHealthAndArmor;
                }
            }
        }
    }

    private int GetPlayerHealthAndArmorValue()
    {
        return Game.Player.Character.Health + Game.Player.Character.Armor;
    }
}