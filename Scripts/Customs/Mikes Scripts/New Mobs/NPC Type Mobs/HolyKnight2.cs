using System;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class HolyKnight2 : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(25.0); // Time between speeches
        private DateTime m_NextSpeechTime;
        private DateTime lastDirectionChange;
        private TimeSpan directionChangeInterval = TimeSpan.FromSeconds(20);
        private static string[] knightNames =
        {
            "SirValor", "LordGuardian", "KnightProtector", "HolyAvenger", "LightBringer", "CrusaderOfJustice",
            "DivineChampion", "NobleDefender", "SaintWarrior", "CelestialKnight", "SacredSavior", "FaithfulPaladin",
            "GloriousKnight", "HeavenlyHero", "JusticeBringer", "VirtuousVanguard", "RighteousDefender", "NobleCrusader",
            "HonorableProtector", "SaintlyWarrior", "DivinePaladin", "HolyCrusader", "ValiantKnight", "GuardianOfLight",
            "BlessedChampion", "ValorKnight", "HeroicDefender", "SacredSentinel", "DivineGuardian", "CelestialProtector"
        };

        private static Type[] weaponTypes = 
        {
            typeof(ErdricksBlade), typeof(Excalibur), typeof(FlamebaneWarAxe), typeof(FrostflameKatana), typeof(GlassSwordOfValor), 
            typeof(IlluminaDagger), typeof(KingsSwordOfHaste), typeof(MageMusher), typeof(MagicAxeOfGreatStrength), typeof(SwordOfGideon)
        };

        public override bool ClickTitle { get { return false; } }

        [Constructable]
        public HolyKnight2() : base(AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomMinMax(1, 3000);
            Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female") + " the " + knightNames[Utility.Random(knightNames.Length)];
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male") + " the " + knightNames[Utility.Random(knightNames.Length)];
            }

            // Enhanced stats for the Holy Knight
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
            Karma = 6000; // Positive Karma for a good NPC

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
            // Plate armor with a holy theme
            if (Utility.RandomBool())
            {
                AddItem(new PlateChest() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateLegs() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateArms() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateGloves() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new PlateHelm() { Hue = Utility.RandomMinMax(1, 3000) });
            }
            else
            {
                AddItem(new ChainChest() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new ChainLegs() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new LeatherGloves() { Hue = Utility.RandomMinMax(1, 3000) });
                AddItem(new ChainCoif() { Hue = Utility.RandomMinMax(1, 3000) });
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
                        case 0: this.Say(true, "For justice and honor!"); break;
                        case 1: this.Say(true, "Evil shall not prevail!"); break;
                        case 2: this.Say(true, "You will face the consequences of your deeds!"); break;
                        case 3: this.Say(true, "The light will guide me to victory!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);  // Standard loot for a noble NPC
        }

        public override bool AlwaysMurderer { get { return false; } }  // This should be false, as the Holy Knight is not a murderer

        public HolyKnight2(Serial serial) : base(serial)
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
