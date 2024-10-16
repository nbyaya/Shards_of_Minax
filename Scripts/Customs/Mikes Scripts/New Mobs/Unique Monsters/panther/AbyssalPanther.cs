using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an abyssal panther corpse")]
    public class AbyssalPanther : BaseCreature
    {
        private DateTime m_NextAbyssalVortex;
        private DateTime m_NextDarkVeil;
        private bool m_IsInvisible;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AbyssalPanther()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Abyssal Panther";
            Body = 0xD6; // Panther body
            Hue = 2186; // Dark purple hue
            BaseSoundID = 0x462; // Panther sound

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

        public AbyssalPanther(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextAbyssalVortex = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Randomize initial interval
                    m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Randomize initial interval
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAbyssalVortex)
                {
                    AbyssalVortex();
                }

                if (DateTime.UtcNow >= m_NextDarkVeil && !m_IsInvisible)
                {
                    DarkVeil();
                }
            }
        }

        private void AbyssalVortex()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates an abyssal vortex *");
            PlaySound(0x15E);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.MovingEffect(this, 0x37CC, 10, 0, false, false);
                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(PullTarget), m);
                }
            }

            m_NextAbyssalVortex = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for AbyssalVortex
        }

        private void PullTarget(object state)
        {
            if (state is Mobile target)
            {
                target.MoveToWorld(GetSpawnPosition(1), Map);
                target.Freeze(TimeSpan.FromSeconds(2));
                target.Damage(Utility.RandomMinMax(20, 30), this);
                target.SendLocalizedMessage(1070847); // The abyssal vortex pulls you in and damages you!
            }
        }

        private void DarkVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Disappears into the shadows *");
            PlaySound(0x22F);

            m_IsInvisible = true;
            Hidden = true;

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(ReappearBehindTarget));

            m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DarkVeil
        }

        private void ReappearBehindTarget()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                Point3D behindTarget;

                // Determine the position behind the target based on direction
                switch (target.GetDirectionTo(this))
                {
                    case Direction.North:
                        behindTarget = new Point3D(target.X, target.Y + 1, target.Z);
                        break;
                    case Direction.South:
                        behindTarget = new Point3D(target.X, target.Y - 1, target.Z);
                        break;
                    case Direction.East:
                        behindTarget = new Point3D(target.X - 1, target.Y, target.Z);
                        break;
                    case Direction.West:
                        behindTarget = new Point3D(target.X + 1, target.Y, target.Z);
                        break;
                    default:
                        behindTarget = target.Location;
                        break;
                }

                MoveToWorld(behindTarget, target.Map);
            }

            m_IsInvisible = false;
            Hidden = false;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Emerges from the shadows *");
            PlaySound(0x15);

            Mobile victim = Combatant as Mobile;

            if (victim != null && victim.Alive)
            {
                DoHarmful(victim);
                victim.Damage(Utility.RandomMinMax(30, 40), this);
                victim.SendLocalizedMessage(1070848); // The Abyssal Panther catches you off guard with a surprise attack!
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p) && !Server.Spells.SpellHelper.CheckMulti(p, Map))
                    return p;
            }

            return Location;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_IsInvisible);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsInvisible = reader.ReadBool();

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
        }
    }
}
