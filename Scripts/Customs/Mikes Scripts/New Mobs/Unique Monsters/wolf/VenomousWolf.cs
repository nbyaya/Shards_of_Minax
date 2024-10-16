using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a venomous wolf corpse")]
    public class VenomousWolf : BaseCreature
    {
        private DateTime m_NextVenomousSpit;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VenomousWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous wolf";
            Body = 23; // Using DireWolf body
            Hue = 2588; // Unique hue for venomous effect
			BaseSoundID = 0xE5;
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

        public VenomousWolf(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextVenomousSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextVenomousSpit)
                {
                    VenomousSpit();
                }
            }
        }

        private void VenomousSpit()
        {
            Point3D location = GetSpawnPosition(2);
            if (location != Point3D.Zero)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The venomous wolf spits a glob of poison *");
                FixedEffect(0x373A, 10, 16);

                Effects.SendLocationParticles(EffectItem.Create(location, Map, EffectItem.DefaultDuration), 0x3799, 10, 20, 0x96, 0, 0, 0);
                
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are caught in the poisonous cloud!");
                        m.Damage(Utility.RandomMinMax(10, 20), this);
                        m.ApplyPoison(this, Poison.Lethal);
                    }
                }

                Random rand = new Random();
                m_NextVenomousSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 120)); // Random cooldown between 1 and 2 minutes
            }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);
            if (Utility.RandomDouble() < 0.2)
            {
                caster.SendMessage("The venomous wolf's bite seems to resist the poison!");
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
            if (willKill)
            {
                from.SendMessage("The venomous wolf's poison clings to your wounds as it dies.");
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The venomous wolf collapses, leaving behind a trail of poison *");
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
