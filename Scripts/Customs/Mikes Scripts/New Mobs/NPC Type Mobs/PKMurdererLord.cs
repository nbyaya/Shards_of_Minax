using System;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class PKMurdererLord : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(25.0); // time between water ninja speech
        public DateTime m_NextSpeechTime;
        private DateTime lastDirectionChange;
        private TimeSpan directionChangeInterval = TimeSpan.FromSeconds(20);
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

        private static Type[] weaponTypes = 
        {
            typeof(AlamoDefendersAxe), typeof(AssassinsKryss), typeof(AtmaBlade), typeof(AxeOfTheJuggernaut), typeof(Blackrazor), 
            typeof(BlackSwordOfMondain), typeof(DawnbreakerMace), typeof(CrissaegrimEdge), typeof(Dreamseeker), typeof(EbonyWarAxeOfVampires)
        };

        public override bool ClickTitle { get { return false; } }

        [Constructable]
        public PKMurdererLord() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomMinMax(1, 3000);
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

            // Enhanced stats for the Lord
            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
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
            EquipRandomWeapon();  // Equip a random weapon
			AddCloak();           // Add the cloak
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
                AddItem(new BoneChest() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new BoneLegs() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new BoneArms() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new BoneGloves() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new BoneHelm() { Hue = Utility.RandomMinMax(1, 3000) });
            }
            else
            {
                AddItem(new PlateChest() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateLegs() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateArms() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateGloves() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateHelm() { Hue = Utility.RandomMinMax(1, 3000) });
            }
        }

        private void EquipRandomWeapon()
        {
            Type weaponType = weaponTypes[Utility.Random(weaponTypes.Length)];
            Item weapon = (Item)Activator.CreateInstance(weaponType);
            weapon.Hue = Utility.RandomMinMax(1, 3000);
            AddItem(weapon);
        }
		
        private void AddCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Hue = Utility.RandomMinMax(1, 3000); // Set hue between 1 and 3000
            AddItem(cloak);
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
            AddLoot(LootPack.UltraRich);  // Even richer loot than before
        }

        public override bool AlwaysMurderer { get { return true; } }

        public PKMurdererLord(Serial serial) : base(serial)
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
