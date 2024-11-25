using System;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class PKMurderer : BaseCreature
    {
		private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(25.0); // time between water ninja speech
        public DateTime m_NextSpeechTime;
		private DateTime lastDirectionChange;
		private TimeSpan directionChangeInterval = TimeSpan.FromSeconds(20);
        // Names to simulate internet trolls.
        private static string[] trollNames = 
        {
		"TrollMaster", "DarkSlaya", "NoobCrusher", "XxKillYouxX", "ShadowFiend", "PKLord",
		"RuthlessKilla", "StealthAssassin", "BloodyMarauder", "SilentStalker", "DeathDealer",
		"XxDeathBringerxX", "VengefulSlayer", "NemesisKnight", "CruelCrusader", "BrutalBerserker",
		"MercilessMarauder", "PvPGod", "FatalFury", "KillerInstinct", "RampageRuler",
		"WarlordWrecker", "Doomsday", "EpicEliminator", "RogueReaper", "ChaoticCrusher",
		"RelentlessRavager", "PredatorPrime", "AlphaAssailant", "BrutalBlitz", "MortalMachete",
		"SavageSlayer", "ProwlerPrime", "ViciousVanquisher", "RuthlessReaver", "NoMercyManiac",
		"DeadlyDesperado", "GrimGuerilla", "FatalFrost", "SearingSun", "WickedWarlock",
		"TerribleTempest", "BlazingBlade", "ColdCarnage", "RagingRiot", "MalignantMystic",
		"FerociousFiend", "DreadfulDominator", "LethalLegend", "GruesomeGhoul", "FatalForce",
		"DoomDeliverer", "SavageSaboteur", "PvPPhantom", "EpicExterminator", "ChaosChampion",
		"DeadlyDreadnought", "GrimGoliath", "FierceFighter", "BrutalBrawler", "MercilessMauler",
		"ProwlingPredator", "VengefulViper", "RuthlessRonin", "NoobNightmare", "XxSlayerKingxX",
		"ShadowSentinel", "StealthSpecter", "BloodyBuccaneer", "SilentSlasher", "DeathDefiler",
		"XxReaperRexxX", "ViciousVandal", "NemesisNinja", "CruelConqueror", "BrutalBarbarian",
		"MercilessMutilator", "PvPParagon", "FuriousFighter", "KillerKhan", "RampageRegent",
		"WarlordWreckage", "DoomsdayDaemon", "EpicEradicator", "RogueRipper", "ChaoticChaos",
		"RelentlessRipper", "PredatorProwler", "AlphaAnnihilator", "BrutalBombardment", "MortalMangler",
		"SavageStalker", "ProwlerPhantom", "ViciousVandaler", "RuthlessRavager", "NoQuarterQuasher",
		"DeadlyDuelist", "GrimGladiator", "FatalFlurry", "SearingSerpent", "WickedWarrior",
		"TerribleTyphoon", "BlazingBerserker", "ColdCalamity", "RagingRampart", "MalignantMonster",
		"FerociousFury", "DreadfulDestroyer", "LethalLeviathan", "GruesomeGhost", "FatalFalcon",
		"DoomDeliverer", "SavageSaboteur", "EpicExterminator", "PhantomFury", "ChaosConqueror",
		"VengefulViper", "NefariousNightmare", "BrutalBrawler", "MercilessMauler", "PvPPhantom",
		"FatalFalcon", "RuthlessRonin", "ShadowSentinel", "CrimsonCarnage", "StealthSpecter",
		"DreadfulDuelist", "LethalLeviathan", "GrimGoliath", "FrostyFear", "SearingSerpent",
		"WickedWarlord", "TerribleTyphoon", "BlazingBrawler", "ColdConqueror", "RagingRogue",
		"MalignantMonarch", "FerociousFang", "DreadfulDragon", "LethalLancer", "GruesomeGhost",
		"FatalFlame", "SavageStorm", "ProwlingPhantom", "ViciousVandal", "RuthlessRipper",
		"NoQuarterQuasher", "DeadlyDagger", "GrimGladiator", "FrostbiteFury", "SunscorchSlayer"
        };


        public override bool ClickTitle { get { return false; } }

        [Constructable]
        public PKMurderer() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female") + " the " + trollNames[Utility.Random(trollNames.Length)];
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male") + " the " + trollNames[Utility.Random(trollNames.Length)];
            }

            // Enhance stats for more power
			SetStr( 800, 1200 );
			SetDex( 177, 255 );
			SetInt( 151, 250 );
			Team = Utility.RandomMinMax(1, 5);

			SetHits( 600, 1000 );

			SetDamage( 10, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Meditation, 25.1, 50.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill(SkillName.Anatomy, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Archery, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.ArmsLore, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Bushido, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Chivalry, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Fencing, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Lumberjacking, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Ninjitsu, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Parry, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Swords, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Tactics, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Wrestling, Utility.RandomMinMax(50, 100));

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -18.9;

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 58;

            AddRandomArmor();

            Utility.AssignRandomHair(this);
			lastDirectionChange = DateTime.Now;
			
			// Add the horse when the creature is created
			AddHorse();
			
			ActiveSpeed = 0.01;   // Default is around 0.2, lower is faster
			PassiveSpeed = 0.02;  // Default is around 0.4, lower is faster
        }

        private void AddRandomArmor()
        {
            // Bone or Plate armor
            if (Utility.RandomBool())
            {
                AddItem(new BoneChest() { Hue = Utility.RandomDyedHue() });
                AddItem(new BoneLegs() { Hue = Utility.RandomDyedHue() });
                AddItem(new BoneArms() { Hue = Utility.RandomDyedHue() });
                AddItem(new BoneGloves() { Hue = Utility.RandomDyedHue() });
                AddItem(new BoneHelm() { Hue = Utility.RandomDyedHue() });
            }
            else
            {
                AddItem(new PlateChest() { Hue = Utility.RandomDyedHue() });
                AddItem(new PlateLegs() { Hue = Utility.RandomDyedHue() });
                AddItem(new PlateArms() { Hue = Utility.RandomDyedHue() });
                AddItem(new PlateGloves() { Hue = Utility.RandomDyedHue() });
                AddItem(new PlateHelm() { Hue = Utility.RandomDyedHue() });
            }

            // Random weapon
            switch (Utility.Random(7))
            {
                case 0: AddItem(new Longsword()); break;
                case 1: AddItem(new Cutlass()); break;
                case 2: AddItem(new Broadsword()); break;
                case 3: AddItem(new Axe()); break;
                case 4: AddItem(new Club()); break;
                case 5: AddItem(new Dagger()); break;
                case 6: AddItem(new Spear()); break;
            }
        }
		
		public void AddHorse()
		{
			Horse mount = new Horse();
			mount.Rider = this; // This automatically mounts the creature on the horse
		}
		
		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now - lastDirectionChange > directionChangeInterval)
			{
				// Randomly change direction
				Direction = (Direction)Utility.Random(8);
				lastDirectionChange = DateTime.Now;
			}

			// Attempt to move in the current direction
			if (!Move(Direction))
			{
				// If movement is blocked, try a different direction
				Direction = (Direction)Utility.Random(8);
			}
		}
		
		public override int Damage(int amount, Mobile from)
		{
			Mobile combatant = this.Combatant as Mobile;

			if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
			{
				if (Utility.RandomBool())
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
						case 0: this.Say(true, "Your skills a low noob!"); break;
						case 1: this.Say(true, "You should have stayed in the safe zone!"); break;
						case 2: this.Say(true, "I'll show you the true meaning of fear!"); break;
						case 3: this.Say(true, "You're just another notch on my belt."); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);  // Richer loot compared to Brigand
        }

        public override bool AlwaysMurderer { get { return true; } }

        public PKMurderer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
