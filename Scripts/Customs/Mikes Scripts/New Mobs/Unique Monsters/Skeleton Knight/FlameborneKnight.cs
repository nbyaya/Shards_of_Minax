using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a flameborne knight corpse")]
    public class FlameborneKnight : BaseCreature
    {
        private static readonly int FlameborneHue = 2372; // Custom hue for Flameborne Knight
        private DateTime m_NextInfernoSlash;
        private DateTime m_NextBlazingAura;
        private bool m_AbilitiesInitialized;
        private const double InfernoSlashDamage = 10.0;
        private const double BlazingAuraRadius = 5.0;

        [Constructable]
        public FlameborneKnight()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flameborne knight";
            Body = 57; // BoneKnight body
            Hue = FlameborneHue; // Custom hue for Flameborne Knight
            BaseSoundID = 451;

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

            m_AbilitiesInitialized = false; // Set flag to false for initialization
        }

        public FlameborneKnight(Serial serial)
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
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextInfernoSlash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextBlazingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextInfernoSlash)
                {
                    InfernoSlash();
                }

                if (DateTime.UtcNow >= m_NextBlazingAura)
                {
                    BlazingAura();
                }
            }
        }

        private void InfernoSlash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flameborne Knight performs an Inferno Slash! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 16, 0xFF, 0); // Flames effect
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(15, 25);
                target.Damage(damage, this);
                target.SendMessage("You are burned by the inferno!");
                Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                {
                    if (target != null && target.Alive)
                    {
                        int burnDamage = Utility.RandomMinMax(5, 10);
                        target.Damage(burnDamage, this);
                        target.SendMessage("You are still burning from the inferno!");
                    }
                });
            }

            // Reset cooldown with fixed duration
            m_NextInfernoSlash = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void BlazingAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flameborne Knight activates its Blazing Aura! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 16, 0xFF, 0); // Flames effect

            foreach (Mobile m in GetMobilesInRange((int)BlazingAuraRadius))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("You are scorched by the blazing aura!");
                    int damage = Utility.RandomMinMax(5, 10);
                    m.Damage(damage, this);
                }
            }

            // Reset cooldown with fixed duration
            m_NextBlazingAura = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
