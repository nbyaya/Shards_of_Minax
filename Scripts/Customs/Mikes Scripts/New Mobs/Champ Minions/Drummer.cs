using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a drummer")]
    public class Drummer : BaseCreature
    {
        private TimeSpan m_BuffDelay = TimeSpan.FromSeconds(20.0); // time between buffs
        public DateTime m_NextBuffTime;
        private bool m_BuffActive = false;

        [Constructable]
        public Drummer() : base(AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Drummer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Drummer";
            }

            Item drum = new Item(0x0E9D); // Item ID for a drum
            drum.Layer = Layer.TwoHanded;
            drum.Movable = false;
            AddItem(drum);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 700);
            SetDex(100, 150);
            SetInt(50, 75);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.Tactics, 70.1, 80.0);
            SetSkill(SkillName.Wrestling, 70.1, 80.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextBuffTime = DateTime.Now + m_BuffDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextBuffTime && !m_BuffActive)
            {
                Effects.PlaySound(Location, Map, 0x1F7); // Drum sound effect
                Say(true, "Feel the rhythm!");

                foreach (Mobile m in GetMobilesInRange(8))
                {
                    if (m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        bc.AddStatMod(new StatMod(StatType.Str, "DrumBuff", 10, TimeSpan.FromSeconds(30.0)));
                        bc.AddStatMod(new StatMod(StatType.Dex, "DrumBuff", 10, TimeSpan.FromSeconds(30.0)));
                        bc.AddStatMod(new StatMod(StatType.Int, "DrumBuff", 10, TimeSpan.FromSeconds(30.0)));
                    }
                }

                m_BuffActive = true;
                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }
            else if (DateTime.Now >= m_NextBuffTime && m_BuffActive)
            {
                m_BuffActive = false;
                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }
        }

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));
            AddLoot(LootPack.Meager);
        }

        public Drummer(Serial serial) : base(serial)
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
