using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a frost wapiti corpse")]
    public class FrostWapiti : BaseCreature
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextIcicleCharge;
        private DateTime m_NextFrostAura;
        private DateTime m_NextSummonMinions;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostWapiti()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frost Wapiti";
            Body = 0xEA; // GreatHart body
            Hue = 1988; // Frosty blue hue

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FrostWapiti(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}
        public override int GetAttackSound() 
        { 
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Initialize random cooldowns
                    Random rand = new Random();
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextIcicleCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextFrostAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextIcicleCharge)
                {
                    IcicleCharge();
                }

                if (DateTime.UtcNow >= m_NextFrostAura)
                {
                    FrostAura();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonIcyMinions();
                }
            }
        }

        private void FrostBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Wapiti exhales a chilling breath! *");
            FixedEffect(0x37C4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && m.InRange(this, 4))
                {
                    m.SendMessage("You feel a freezing chill as the Frost Wapiti breathes icy air!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.SendMessage("Your movements are slowed by the icy breath!");
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void IcicleCharge()
        {
            if (Combatant == null || !InRange(Combatant, 2))
                return;

            Mobile mobile = Combatant as Mobile; // Ensure Combatant is of type Mobile
            if (mobile != null) // Check if the cast succeeded
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Wapiti's charge freezes you in your tracks! *");
                FixedEffect(0x376A, 10, 16);
                mobile.SendMessage("The Frost Wapiti charges at you with icy force!");
                mobile.Freeze(TimeSpan.FromSeconds(2));
                mobile.Damage(Utility.RandomMinMax(20, 30), this);

                // Knock back effect
                int x = mobile.X + Utility.RandomMinMax(-2, 2);
                int y = mobile.Y + Utility.RandomMinMax(-2, 2);
                int z = Map.GetAverageZ(x, y);

                mobile.Location = new Point3D(x, y, z);
                mobile.SendMessage("You are knocked back by the force of the charge!");

                // Create icy area effect
                Effects.SendLocationEffect(new Point3D(x, y, z), Map, 0x36BD, 20, 10);
            }

            m_NextIcicleCharge = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void FrostAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Wapiti's aura chills the air around it! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.InRange(this, 5))
                {
                    m.SendMessage("You are chilled by the aura of the Frost Wapiti!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                    m.Freeze(TimeSpan.FromSeconds(1));
                }
            }

            m_NextFrostAura = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void SummonIcyMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Wapiti summons icy minions to aid in the fight! *");
            FixedEffect(0x376A, 10, 16);

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
                if (Map.CanSpawnMobile(loc))
                {
                    IcyMinion minion = new IcyMinion();
                    minion.MoveToWorld(loc, Map);
                }
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            // Reset abilities initialization flag and randomize intervals
            m_AbilitiesInitialized = false;
            m_NextFrostBreath = DateTime.UtcNow;
            m_NextIcicleCharge = DateTime.UtcNow;
            m_NextFrostAura = DateTime.UtcNow;
            m_NextSummonMinions = DateTime.UtcNow;
        }
    }

    public class IcyMinion : BaseCreature
    {
        [Constructable]
        public IcyMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an icy minion";
            Body = 0x13D; // Human body as base
            Hue = 1153; // Frosty blue hue

            SetStr(50, 70);
            SetDex(50, 70);
            SetInt(30, 50);

            SetHits(60, 80);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Cold, 40, 50);

            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 20;
        }

        public IcyMinion(Serial serial)
            : base(serial)
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
