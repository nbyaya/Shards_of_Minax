using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a flamebringer ogre corpse")]
    public class FlamebringerOgre : BaseCreature
    {
        private DateTime m_NextFireball;
        private DateTime m_NextInfernoStrike;
        private DateTime m_NextFlamingAura;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public FlamebringerOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flamebringer ogre";
            Body = 1; // Ogre body
            Hue = 1350; // Unique hue for fiery appearance
            BaseSoundID = 427;

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

            PackItem(new Club());

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public FlamebringerOgre(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextFireball = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInfernoStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextFlamingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFireball)
                {
                    FireballThrow();
                }

                if (DateTime.UtcNow >= m_NextInfernoStrike)
                {
                    InfernoStrike();
                }

                if (DateTime.UtcNow >= m_NextFlamingAura)
                {
                    FlamingAura();
                }
            }
        }

        private void FireballThrow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flamebringer Ogre hurls a fireball! *");

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                // Create fireball projectile
                Effects.SendTargetEffect(target, 0x36D4, 16);
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (target.InRange(this, 10))
                    {
                        target.FixedEffect(0x3709, 10, 16);
                        target.Damage(Utility.RandomMinMax(10, 20), this);

                        // Spawn HotLavaTile items
                        for (int i = 0; i < 3; i++)
                        {
                            Point3D loc = new Point3D(target.X + Utility.RandomMinMax(-1, 1), target.Y + Utility.RandomMinMax(-1, 1), target.Z);
                            HotLavaTile lava = new HotLavaTile();
                            lava.MoveToWorld(loc, Map);
                        }
                    }
                });

                m_NextFireball = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set the cooldown for the next FireballThrow
            }
        }

        private void InfernoStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flamebringer Ogre swings with fiery rage! *");

            if (Utility.RandomDouble() < 0.3)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.Damage(Utility.RandomMinMax(5, 10), this);
                    target.SendMessage("You are ignited by the Flamebringer Ogre's fiery strike!");
                    target.FixedEffect(0x36D4, 10, 16);
                }
            }

            m_NextInfernoStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Set the cooldown for the next InfernoStrike
        }

        private void FlamingAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flamebringer Ogre radiates a scorching aura! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                    m.SendMessage("You are burned by the Flamebringer Ogre's fiery aura!");
                    m.FixedEffect(0x36D4, 10, 16);
                }
            }

            m_NextFlamingAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set the cooldown for the next FlamingAura
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
