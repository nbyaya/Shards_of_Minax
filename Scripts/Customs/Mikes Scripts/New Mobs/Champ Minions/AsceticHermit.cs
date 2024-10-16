using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an ascetic hermit")]
    public class AsceticHermit : BaseCreature
    {
        private TimeSpan m_BuffDelay = TimeSpan.FromSeconds(20.0); // time between hermit buffs
        public DateTime m_NextBuffTime;
        private TimeSpan m_AreaDenialDelay = TimeSpan.FromSeconds(30.0); // time between area denial actions
        public DateTime m_NextAreaDenialTime;

        [Constructable]
        public AsceticHermit() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Ascetic Hermit";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Ascetic Hermit";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item sandals = new Sandals();
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(sandals);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(400, 600);
            SetDex(100, 150);
            SetInt(300, 400);

            SetHits(500, 700);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 6000;
            Karma = 6000;

            VirtualArmor = 50;

            m_NextBuffTime = DateTime.Now + m_BuffDelay;
            m_NextAreaDenialTime = DateTime.Now + m_AreaDenialDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBuffTime)
            {
                ApplyPassiveBuffs();
                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }

            if (DateTime.Now >= m_NextAreaDenialTime)
            {
                ApplyAreaDenial();
                m_NextAreaDenialTime = DateTime.Now + m_AreaDenialDelay;
            }

            base.OnThink();
        }

        private void ApplyPassiveBuffs()
        {
            this.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist);
            this.Say(true, "Feel the serenity of my meditation!");

            // Example buff: Increase physical resistance temporarily
        }

        private void ApplyAreaDenial()
        {
            this.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
            this.Say(true, "You shall not pass!");

            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && m is BaseCreature && !m.Hidden)
                {
                    m.SendMessage("You are pushed back by an unseen force!");
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-1, 1), m.Y + Utility.RandomMinMax(-1, 1), m.Z);
                }
            }
        }

        public AsceticHermit(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            PackGold(150, 200);
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
