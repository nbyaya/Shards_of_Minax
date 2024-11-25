using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a quantum physicist")]
    public class QuantumPhysicist : BaseCreature
    {
        private TimeSpan m_AnomalyDelay = TimeSpan.FromSeconds(20.0); // time between anomaly creation
        public DateTime m_NextAnomalyTime;

        [Constructable]
        public QuantumPhysicist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Quantum Physicist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Quantum Physicist";
            }

            Item labCoat = new Robe();
            labCoat.Hue = Utility.RandomNeutralHue();
            AddItem(labCoat);

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

            SetStr(300, 400);
            SetDex(90, 120);
            SetInt(400, 600);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 50);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Tamable = false;

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextAnomalyTime = DateTime.Now + m_AnomalyDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextAnomalyTime)
            {
                CreateQuantumAnomaly();
                m_NextAnomalyTime = DateTime.Now + m_AnomalyDelay;
            }

            base.OnThink();
        }

        private void CreateQuantumAnomaly()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
            {
                this.Say(true, "Feel the power of quantum anomalies!");

                // Create an anomaly that disrupts movement (example implementation, actual effect logic should be defined)
                combatant.SendMessage("You are caught in a quantum anomaly, your movements are disrupted!");
                combatant.Freeze(TimeSpan.FromSeconds(5.0)); // Freezes the target for 5 seconds
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            PackItem(new BlackPearl(Utility.RandomMinMax(5, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 15)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
            {
                this.Say(true, "Your attacks are futile against quantum physics!");
            }

            return base.Damage(amount, from);
        }

        public QuantumPhysicist(Serial serial) : base(serial)
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
