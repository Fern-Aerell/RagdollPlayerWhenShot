using System;
using GTA;
using GTA.Math;
using GTA.Native;

public class RagdollPlayerWhenShot : Script
{
    private readonly string version = "1.0.0";
    private int lastHealthAndArmor;
    private readonly Random random = new Random();

    public RagdollPlayerWhenShot()
    {
        Tick += OnTick;
        lastHealthAndArmor = GetPlayerHealthAndArmorValue();
        ShowWatermark();
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Game.Player.Character.IsAlive)
        {
            int currentHealthAndArmor = GetPlayerHealthAndArmorValue();

            if (currentHealthAndArmor < lastHealthAndArmor && IsPlayerHitByBullet())
            {
                int minTime = random.Next(500, 1000);
                Game.Player.Character.SetToRagdoll(minTime, minTime * 5, RagdollType.Balance, true);
            }

            lastHealthAndArmor = currentHealthAndArmor;
        }
    }

    private bool IsPlayerHitByBullet()
    {
        foreach (var ped in World.GetAllPeds())
        {
            if (ped.IsShooting && IsBulletHittingPlayer(ped.Position + new Vector3(0, 0, 1)))
                return true;
        }
        return false;
    }

    private bool IsBulletHittingPlayer(Vector3 bulletOrigin)
    {
        var result = World.Raycast(bulletOrigin, Game.Player.Character.Position, IntersectFlags.PedCapsules);
        return result.DidHit && result.HitEntity == Game.Player.Character;
    }

    private int GetPlayerHealthAndArmorValue()
    {
        return Game.Player.Character.Health + Game.Player.Character.Armor;
    }

    private void ShowWatermark()
    {
        Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
        Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, $"RagdollPlayerWhenShot Script V{version} By Fern Aerell.");
        Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, false, true, -1);
    }
}