using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System;
using Ultima;
using Server.Engines.XmlSpawner2;

namespace Bittiez.CustomLoot
{
    public static class CustomLootSystem
    {
        public static void Initialize()
        {
            EventSink.CreatureDeath += EventSink_CreatureDeath;
			// Initialize the tension system as well.
			Bittiez.CustomSystems.TensionDeathHandler.Initialize();			
        }

		private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
		{
			if (e.Creature == null)
				return;

			// Ignore summoned creatures (temporary summons shouldn't give XP)
			if (e.Creature is BaseCreature baseCreature && baseCreature.Summoned)
				return;

			PlayerMobile player = null;

			// Check if the killer is a player or a player's controlled creature
			if (e.Killer is PlayerMobile directPlayer)
			{
				player = directPlayer;
			}
			else if (e.Killer is BaseCreature killerCreature && killerCreature.Controlled && killerCreature.ControlMaster is PlayerMobile master)
			{
				player = master; // Assign XP to the master of the pet
			}

			if (player != null)
			{
				var profile = player.AcquireTalents();
				int xpAward = CalcExp(e.Creature); // Use the new XP formula

				profile.XP += xpAward;
				player.SendMessage($"You gained {xpAward} XP from defeating {e.Creature.Name}.");

				// Check for level-up(s)
				while (profile.XP >= Talents.GetXPThresholdForLevel(profile.Level + 1))
				{
					profile.Level++;

					if (!profile.Talents.TryGetValue(TalentID.AncientKnowledge, out var talent))
					{
						talent = new Talent(TalentID.AncientKnowledge);
						profile.Talents[TalentID.AncientKnowledge] = talent;
					}
					talent.Points += 5;
					player.SendMessage($"Congratulations! You've reached level {profile.Level} and received 5 Maxxia points!");
				}

				// Award XP to levelable gear (if any)
				AwardXPToLevelableItems(player, e.Creature);

				// If the kill was made by a pet, attempt to gain Animal Taming and Animal Lore.
				if (e.Killer is BaseCreature pet && pet.Controlled)
				{
					// Animal-related skills gains
					SkillCheck.Gain(player, player.Skills[SkillName.AnimalTaming]);
					SkillCheck.Gain(player, player.Skills[SkillName.AnimalLore]);
					player.SendMessage("Your pet's performance has improved your Animal Taming and Animal Lore skills!");

					// Check if the player is wielding a Shepherds Crook to also gain Herding.
					Item heldItem = player.FindItemOnLayer(Layer.OneHanded);
					if (heldItem == null)
						heldItem = player.FindItemOnLayer(Layer.TwoHanded);

					if (heldItem != null && heldItem is ShepherdsCrook)
					{
						SkillCheck.Gain(player, player.Skills[SkillName.Herding]);
						player.SendMessage("Your Shepherds Crook aids your Herding training!");
					}
				}
			}
		}



        private static int CalcExp(Mobile targ)
        {
            double val = targ.Hits + targ.Stam + targ.Mana;

            for (int i = 0; i < targ.Skills.Length; i++)
                val += targ.Skills[i].Base;

            if (val > 700)
                val = 700 + ((val - 700) / 3.66667);

            BaseCreature bc = targ as BaseCreature;

            if (IsMageryCreature(bc))
                val += 100;

            if (IsPoisonImmune(bc))
                val += 100;

            if (targ is VampireBat || targ is VampireBatFamiliar)
                val += 100;

            val += GetPoisonLevel(bc) * 20;

            val /= 10;

            return (int)val;
        }

		private static bool IsMageryCreature( BaseCreature bc )
		{
			return ( bc != null && bc.AI == AIType.AI_Mage && bc.Skills[SkillName.Magery].Base > 5.0 );
		}

		private static bool IsPoisonImmune( BaseCreature bc )
		{
			return ( bc != null && bc.PoisonImmune != null );
		}

		private static int GetPoisonLevel( BaseCreature bc )
		{
			if ( bc == null )
				return 0;

			Poison p = bc.HitPoison;

			if ( p == null )
				return 0;

			return p.RealLevel + 1;
		}

        private static void AwardXPToLevelableItems(PlayerMobile player, Mobile killed)
        {
            for (int i = 0; i < 25; ++i) // Iterate over equipment layers
            {
                Item item = player.FindItemOnLayer((Layer)i);

                if (item != null)
                {
                    XmlLevelItem levitem = XmlAttach.FindAttachment(item, typeof(XmlLevelItem)) as XmlLevelItem;

                    if (levitem != null)
                    {
                        int exp = CalcExp(killed);
                        int oldLevel = levitem.Level;
                        int expcap = LevelItemManager.CalcExpCap(oldLevel);

                        if (LevelItems.EnableExpCap && exp > expcap)
                            exp = expcap;

                        levitem.Experience += exp;
                        LevelItemManager.InvalidateLevel(levitem);

                        if (levitem.Level != oldLevel)
                            LevelItemManager.OnLevel(levitem, oldLevel, levitem.Level, player);

                        item.InvalidateProperties();
                    }
                }
            }
        }
    }
}
