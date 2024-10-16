using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a molten slime corpse")]
    public class MoltenSlime : BaseCreature
    {
        private DateTime m_NextLavaPool;
        private DateTime m_NextEruptingSlam;
        private DateTime m_NextFieryVortex;
		private DateTime m_NextThink;
        private bool m_AbilitiesInitialized;
        private Point3D m_LastLocation;

        [Constructable]
        public MoltenSlime()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a molten slime";
            Body = 51; // Slime body
            Hue = 2382; // Unique hue for molten effect
			BaseSoundID = 456;

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

            m_AbilitiesInitialized = false;
            m_LastLocation = Location; // Initialize last location
        }

        public MoltenSlime(Serial serial)
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
			if (DateTime.UtcNow < m_NextThink)
				return;

			// Existing logic...
			m_NextThink = DateTime.UtcNow + TimeSpan.FromMilliseconds(100);
            // Check if the creature has moved and leave a lava pool if it has
            if (m_LastLocation != Location)
            {
                LeaveLavaPool();
                m_LastLocation = Location;
            }

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextLavaPool = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEruptingSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFieryVortex = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLavaPool)
                {
                    LeaveLavaPool();
                    m_NextLavaPool = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }

                if (DateTime.UtcNow >= m_NextEruptingSlam)
                {
                    EruptingSlam();
                    m_NextEruptingSlam = DateTime.UtcNow + TimeSpan.FromSeconds(40);
                }

                if (DateTime.UtcNow >= m_NextFieryVortex)
                {
                    FieryVortex();
                    m_NextFieryVortex = DateTime.UtcNow + TimeSpan.FromSeconds(50);
                }
            }
        }

        private void LeaveLavaPool()
        {

            Point3D location = Location;
            HotLavaTile lavaPool = new HotLavaTile();
            lavaPool.MoveToWorld(location, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(5), () => lavaPool.Delete());
        }

        private void EruptingSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Molten Slime slams the ground with a fiery eruption! *");
            PlaySound(0x307); // Eruption sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by a fiery shockwave!");
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Shockwave effect
        }

		private void FieryVortex()
		{
			if (Deleted || !Alive) return;  // Add checks to ensure that the creature is still valid

			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Molten Slime summons a vortex of flames! *");
			PlaySound(0x307); // Vortex sound

			foreach (Mobile m in GetMobilesInRange(5))
			{
				if (m != this && m.Alive && CanBeHarmful(m))
				{
					// Pulling effect with a safe delay
					Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
					{
						if (m != null && m.Alive)
						{
							m.MoveToWorld(Location, Map);
							AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
							m.SendMessage("You are pulled into the fiery vortex!");
						}
					});
				}
			}
		}


        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Molten Core: Explosion on death
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Molten Slime erupts in a fiery explosion! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are scorched by the Molten Slime's explosion!");
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (willKill && from != null)
            {
                // Apply Burning Touch
                from.SendMessage("You are burned by the Molten Slime's lingering fire!");
                AOS.Damage(from, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            }

            base.OnDamage(amount, from, willKill);
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
            m_AbilitiesInitialized = false;
            m_LastLocation = Location; // Reset last location after deserialization
        }
    }

}
