using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Vortex Crab corpse")]
    public class VortexCrab : BaseMount
    {
        private DateTime m_NextCycloneGrasp;
        private DateTime m_NextGaleForceStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VortexCrab()
            : this("Vortex Crab")
        {
        }

        [Constructable]
        public VortexCrab(string name)
            : base(name, 1510, 16081, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4F2;
            Hue = 1396; // Iridescent blue hue

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
            SetResistance(ResistanceType.Poison, 100);
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

        public VortexCrab(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                Mobile mobileTarget = Combatant as Mobile;

                if (mobileTarget != null)
                {
                    if (!m_AbilitiesInitialized)
                    {
                        // Set random intervals for abilities
                        Random rand = new Random();
                        m_NextCycloneGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                        m_NextGaleForceStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                        m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                    }

                    if (DateTime.UtcNow >= m_NextCycloneGrasp)
                    {
                        CycloneGrasp(mobileTarget);
                    }

                    if (DateTime.UtcNow >= m_NextGaleForceStrike)
                    {
                        GaleForceStrike(mobileTarget);
                    }
                }
            }
        }

        public void CycloneGrasp(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 10))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cyclone Grasp! *");
            PlaySound(0x655);

            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            target.PlaySound(0x665);

            target.SendMessage("You are caught in the Vortex Crab's cyclone!");
            target.Paralyze(TimeSpan.FromSeconds(5));

            m_NextCycloneGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update with fixed cooldown after use
        }

        public void GaleForceStrike(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 10))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Gale Force Strike! *");
            PlaySound(0x665);

            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            target.PlaySound(0x665);

            int damage = Utility.RandomMinMax(20, 30);
            AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

            if (target.Paralyzed)
            {
                Point3D knockbackLocation = GetKnockbackLocation(target);
                target.MoveToWorld(knockbackLocation, target.Map);
                target.Animate(21, 6, 1, true, false, 0);
            }

            m_NextGaleForceStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Update with fixed cooldown after use
        }

        private Point3D GetKnockbackLocation(Mobile target)
        {
            Direction d = GetDirectionTo(target);
            int x = target.X, y = target.Y;

            Movement.Movement.Offset(d, ref x, ref y);
            Movement.Movement.Offset(d, ref x, ref y);

            return new Point3D(x, y, target.Map.GetAverageZ(x, y));
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
            m_NextCycloneGrasp = DateTime.UtcNow; // Reset timers to ensure correct behavior
            m_NextGaleForceStrike = DateTime.UtcNow;
        }
    }
}
