using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a vile toad corpse")]
    public class VileToad : BaseCreature
    {
        private DateTime m_NextNoxiousAura;
        private DateTime m_NextToxicSpit;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VileToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vile toad";
            Body = 80; // Giant Toad Body
            Hue = 2442; // Unique hue (use your preferred hue)
            BaseSoundID = 0x26B;

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

        public VileToad(Serial serial) : base(serial)
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
                    m_NextNoxiousAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNoxiousAura)
                {
                    NoxiousAura();
                }

                if (DateTime.UtcNow >= m_NextToxicSpit)
                {
                    ToxicSpit();
                }
            }
        }

        private void NoxiousAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Vile Toad emits a noxious aura! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("You are poisoned by the Vile Toad's aura!");
                    m.ApplyPoison(this, Poison.Greater); // Apply poison
                    m.SendMessage("You feel less able to heal yourself!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            // Reset the next use time with a fixed cooldown
            m_NextNoxiousAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ToxicSpit()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Vile Toad spits toxic venom! *");
                FixedEffect(0x373A, 10, 16);
                target.SendMessage("You are hit by the Vile Toad's toxic spit!");
                target.ApplyPoison(this, Poison.Greater); // Apply poison

                Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                {
                    if (target != null && target.Alive)
                    {
                        target.SendMessage("The poison from the toxic spit lingers and weakens you.");
                        target.Damage(Utility.RandomMinMax(10, 20), this);
                    }
                });

                // Reset the next use time with a fixed cooldown
                m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
