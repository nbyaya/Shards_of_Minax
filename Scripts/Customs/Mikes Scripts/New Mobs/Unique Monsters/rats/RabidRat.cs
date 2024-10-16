using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a rabid rat corpse")]
    public class RabidRat : GiantRat
    {
        private DateTime m_NextFrenziedBite;
        private DateTime m_NextPoisonCloud;
        private DateTime m_NextFrighteningScreech;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RabidRat() : base()
        {
            Name = "a rabid rat";
            Body = 0xD7; // Giant rat body
            Hue = 2264; // Unique hue

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public RabidRat(Serial serial) : base(serial)
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
                    m_NextFrenziedBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrighteningScreech = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrenziedBite)
                {
                    PerformFrenziedBite();
                }

                if (DateTime.UtcNow >= m_NextPoisonCloud)
                {
                    CastPoisonCloud();
                }

                if (DateTime.UtcNow >= m_NextFrighteningScreech)
                {
                    PerformFrighteningScreech();
                }
            }
        }

        private void PerformFrenziedBite()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rabid Rat lunges at you with frenzied fury! *");
                target.PlaySound(0x1E3);
                FixedEffect(0x376A, 10, 16);

                IncreaseAttackSpeed();
                ApplyConfusion(target);

                m_NextFrenziedBite = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        private void IncreaseAttackSpeed()
        {
            // Implementation of temporary boost in attack speed
        }

        private void ApplyConfusion(Mobile target)
        {
            if (target is PlayerMobile player)
            {
                player.SendMessage("You feel disoriented and your movements become erratic!");
                player.SendLocalizedMessage(1060735); // "You are disoriented."
                // Implement additional effects like reduced accuracy or increased spellcasting delay
            }
        }

        private void CastPoisonCloud()
        {
            Point3D location = this.Location;
            Map map = this.Map;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rabid Rat releases a noxious cloud of poison! *");
            Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x36D4, 10, 20, 0x4C4, 0, 9502, 0);
            PlaySound(0x2D6);

            foreach (Mobile mob in GetMobilesInRange(3))
            {
                if (mob != this && mob.Alive && !mob.Player) // Do not affect players or the rat itself
                {
                    if (Utility.RandomDouble() < 0.5) // 50% chance of being poisoned
                    {
                        mob.SendMessage("You are enveloped in the poisonous cloud!");
                        mob.ApplyPoison(this, Poison.Lethal); // Apply lethal poison
                    }
                }
            }

            m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void PerformFrighteningScreech()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rabid Rat lets out a terrifying screech! *");
            PlaySound(0x1D6);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile mob in GetMobilesInRange(5))
            {
                if (mob != this && mob.Alive)
                {
                    mob.SendMessage("You are frightened by the Rabid Rat's screech!");
                    mob.Freeze(TimeSpan.FromSeconds(3)); // Freeze them for 3 seconds
                }
            }

            m_NextFrighteningScreech = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
