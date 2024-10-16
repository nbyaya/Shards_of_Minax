using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a baroque barbarian")]
    public class BaroqueBarbarian : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(15.0); // time between speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public BaroqueBarbarian() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.5)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Baroque Barbarian";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Baroque Barbarian";
            }

            // Fancy Armor
            AddItem(new PlateChest());
            AddItem(new PlateArms());
            AddItem(new PlateGloves());
            AddItem(new PlateLegs());
            AddItem(new Boots());

            // Ruffled Shirt
            AddItem(new FancyShirt(Utility.RandomBlueHue()));

            // Gilded Weapon
            Item weapon = new Broadsword();
            weapon.LootType = LootType.Blessed;
            weapon.Hue = 0x501; // Gold hue
            AddItem(weapon);

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

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override int TreasureMapLevel { get { return 5; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: Say(true, "To battle, with elegance!"); break;
                        case 1: Say(true, "Savage yet sophisticated!"); break;
                        case 2: Say(true, "Barbaric beauty meets you in battle!"); break;
                        case 3: Say(true, "I'll strike you down with style!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGold(300, 400);
            AddLoot(LootPack.UltraRich, 2);

            if (0.2 > Utility.RandomDouble())
            {
                PackItem(new FancyShirt());
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: Say(true, "You scratch my perfect armor!"); break;
                        case 1: Say(true, "You'll pay for that, uncouth one!"); break;
                        case 2: Say(true, "My style is undeniable!"); break;
                        case 3: Say(true, "Your moves are so... pedestrian!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public BaroqueBarbarian(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
