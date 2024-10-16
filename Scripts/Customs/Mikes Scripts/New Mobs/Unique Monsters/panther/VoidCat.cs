using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a void stalker corpse")]
    public class VoidCat : BaseCreature
    {
        private DateTime m_NextVoidWalk;
        private DateTime m_NextEclipseAura;
        private bool m_EclipseAuraActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VoidCat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Void Cat";
            Body = 0xD6; // Panther body
            Hue = 2178; // Dark purple-black hue
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

        public VoidCat(Serial serial)
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
                    m_NextVoidWalk = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEclipseAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVoidWalk)
                {
                    VoidWalk();
                }

                if (DateTime.UtcNow >= m_NextEclipseAura && !m_EclipseAuraActive)
                {
                    EclipseAura();
                }
            }
        }

        private void VoidWalk()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Phases through reality *");
            PlaySound(0x1FE);

            Point3D oldLocation = Location;
            Map oldMap = Map;

            // Find a valid location behind the target
            Mobile target = Combatant as Mobile;

            if (target != null)
            {
                Point3D newLocation;

                // Determine the position behind the target based on direction
                switch (target.GetDirectionTo(this))
                {
                    case Direction.North:
                        newLocation = new Point3D(target.X, target.Y - 1, target.Z);
                        break;
                    case Direction.South:
                        newLocation = new Point3D(target.X, target.Y + 1, target.Z);
                        break;
                    case Direction.East:
                        newLocation = new Point3D(target.X + 1, target.Y, target.Z);
                        break;
                    case Direction.West:
                        newLocation = new Point3D(target.X - 1, target.Y, target.Z);
                        break;
                    default:
                        newLocation = target.Location;
                        break;
                }

                // Perform the teleport
                Map.GetAverageZ(newLocation.X, newLocation.Y);
                MoveToWorld(newLocation, target.Map);

                // Visual effects
                Effects.SendLocationParticles(EffectItem.Create(oldLocation, oldMap, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                Effects.SendLocationParticles(EffectItem.Create(newLocation, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                // Sneak attack
                if (target.Alive)
                {
                    target.PlaySound(0x1F1);
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 40), 100, 0, 0, 0, 0);
                    target.SendLocalizedMessage(1070847); // The Void Stalker strikes from behind!
                }
            }

            m_NextVoidWalk = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown after use
        }

        private void EclipseAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons a veil of darkness *");
            PlaySound(0x108);

            m_EclipseAuraActive = true;

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    m.SendLocalizedMessage(1070846); // The area around you darkens!
                    m.SendMessage("Your vision is impaired by the darkness.");
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(DeactivateEclipseAura));

            m_NextEclipseAura = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Update cooldown after use
        }

        private void DeactivateEclipseAura()
        {
            m_EclipseAuraActive = false;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The darkness dissipates *");

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The disorienting darkness fades away.");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_EclipseAuraActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_EclipseAuraActive = reader.ReadBool();

            m_AbilitiesInitialized = false; // Reset flag to reinitialize intervals
        }
    }
}
