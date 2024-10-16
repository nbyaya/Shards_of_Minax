using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Overlord Mk II")]
    public class OverlordMkII : ExodusMinion
    {
        private DateTime m_NextLaserBarrage;
        private DateTime m_NextStrategicCommand;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public OverlordMkII() : base()
        {
            Name = "Overlord Mk II";
            Body = 0x2F5; // ExodusMinion body
            Hue = 2500; // Unique hue (adjust as needed)

            SetStr(1000, 1200);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(8000, 10000);

            SetDamage(25, 35);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);

            Fame = 28000;
            Karma = -28000;

            VirtualArmor = 80;

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public OverlordMkII(Serial serial) : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0x218;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x232;
        }

        public override int GetHurtSound()
        {
            return 0x140;
        }
        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextLaserBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStrategicCommand = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLaserBarrage)
                {
                    LaserBarrage();
                }

                if (DateTime.UtcNow >= m_NextStrategicCommand)
                {
                    StrategicCommand();
                }
            }
        }

        public void LaserBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x21, true, "* Initiating Laser Barrage *");
            PlaySound(0x208); // Laser sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m.PlaySound(0x208);
                    AOS.Damage(m, this, Utility.RandomMinMax(50, 80), 0, 0, 0, 0, 100);
                }
            }

            Random rand = new Random();
            m_NextLaserBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Random cooldown between 30 and 60 seconds
        }

        public void StrategicCommand()
        {
            PublicOverheadMessage(MessageType.Regular, 0x21, true, "* Activating Strategic Command *");
            PlaySound(0x2A); // Command sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m is BaseCreature bc)
                {
                    if (bc.Controlled || bc.Summoned)
                        continue;

                    bc.PlaySound(0x2A);
                    bc.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                    // Apply temporary buff
                    bc.DamageMin += 5;
                    bc.DamageMax += 10;

                    Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                    {
                        // Remove temporary buff
                        bc.DamageMin -= 5;
                        bc.DamageMax -= 10;
                    });
                }
            }

            Random rand = new Random();
            m_NextStrategicCommand = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(50, 80)); // Random cooldown between 50 and 80 seconds
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialization
        }
    }
}
