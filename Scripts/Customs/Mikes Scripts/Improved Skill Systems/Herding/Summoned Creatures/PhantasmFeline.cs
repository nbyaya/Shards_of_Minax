using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a phantasm feline corpse")]
    public class PhantasmFeline : BaseCreature
    {
        private int m_LivesRemaining;
        private DateTime m_NextPhaseShift;

        [Constructable]
        public PhantasmFeline()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a phantasm feline";
            Body = 0xC9;
            Hue = 1153; // Ghostly blue hue
            BaseSoundID = 0x69;

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_LivesRemaining = 1;
            m_NextPhaseShift = DateTime.UtcNow;
        }

        public PhantasmFeline(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextPhaseShift)
            {
                PhaseShift();
            }
        }

        public override void OnDeath(Container c)
        {
            if (m_LivesRemaining > 0)
            {
                ReviveCreature();
            }
            else
            {
                base.OnDeath(c);
            }
        }

        private void ReviveCreature()
        {
            Hits = HitsMax / 2;
            Stam = StamMax;
            Mana = ManaMax;

            Poison = null;

            Warmode = true;

            m_LivesRemaining--;

            PlaySound(0x655);
            FixedParticles(0x376A, 1, 29, 9962, 33, 0, EffectLayer.Waist);

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Phantasm Feline uses one of its nine lives! *");
        }

        private void PhaseShift()
        {
            if (!Alive || Deleted)
                return;

            Map map = Map;

            if (map == null)
                return;

            int newX = X + Utility.RandomMinMax(-1, 1);
            int newY = Y + Utility.RandomMinMax(-1, 1);
            int newZ = map.GetAverageZ(newX, newY);

            Point3D newLocation = new Point3D(newX, newY, newZ);

            if (map.CanFit(newLocation, 16, false, false))
            {
                Location = newLocation;
                PlaySound(0x1FE);
                FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            }

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_LivesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_LivesRemaining = reader.ReadInt();

            m_NextPhaseShift = DateTime.UtcNow;
        }
    }
}