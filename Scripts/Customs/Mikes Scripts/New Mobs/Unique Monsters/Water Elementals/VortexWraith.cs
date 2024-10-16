using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a vortex wraith corpse")]
    public class VortexWraith : BaseCreature
    {
        private DateTime m_NextWaterVortex;
        private DateTime m_NextChaoticSurge;
        private DateTime m_NextMistform;

        private bool m_AbilitiesInitialized; // Flag to check if abilities have been initialized

        [Constructable]
        public VortexWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vortex wraith";
            Body = 16; // Water Elemental body
            BaseSoundID = 278;
			Hue = 2467; // Blue hue for storm effect

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
            CanSwim = true;

            m_AbilitiesInitialized = false; // Set the initialization flag
        }

        public VortexWraith(Serial serial)
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
                    m_NextWaterVortex = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextChaoticSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMistform = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWaterVortex)
                {
                    WaterVortex();
                }

                if (DateTime.UtcNow >= m_NextChaoticSurge)
                {
                    ChaoticSurge();
                }

                if (DateTime.UtcNow >= m_NextMistform)
                {
                    Mistform();
                }
            }
        }

        private void WaterVortex()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant && m.Player)
                {
                    m.SendMessage("You are pulled into a swirling vortex of water!");
                    m.MoveToWorld(new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z), Map);
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }

            m_NextWaterVortex = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for WaterVortex
        }

        private void ChaoticSurge()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    int effect = Utility.Random(3);
                    switch (effect)
                    {
                        case 0:
                            m.SendMessage("You are scorched by a burst of fire!");
                            m.Damage(Utility.RandomMinMax(10, 15), this);
                            break;
                        case 1:
                            m.SendMessage("You are chilled by a blast of ice!");
                            m.Freeze(TimeSpan.FromSeconds(3));
                            break;
                        case 2:
                            m.SendMessage("You feel a wave of poisonous energy!");
                            m.Damage(Utility.RandomMinMax(5, 10), this);
                            break;
                    }
                }
            }

            m_NextChaoticSurge = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for ChaoticSurge
        }

        private void Mistform()
        {
            this.Hidden = true; // Set Hidden to true
            this.SendMessage("The Vortex Wraith becomes intangible, avoiding all attacks!");

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(EndMistform));
            m_NextMistform = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Mistform
        }

        private void EndMistform()
        {
            this.Hidden = false; // Set Hidden to false
            this.SendMessage("The Vortex Wraith reappears in its solid form.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write(m_NextWaterVortex);
            writer.Write(m_NextChaoticSurge);
            writer.Write(m_NextMistform);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextWaterVortex = reader.ReadDateTime();
            m_NextChaoticSurge = reader.ReadDateTime();
            m_NextMistform = reader.ReadDateTime();

            m_AbilitiesInitialized = false; // Reset flag on deserialize to reinitialize
        }
    }
}
