using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a flickering remnant corpse")]
    public class FlickeringRemnant : BaseCreature
    {
        private DateTime m_NextPulse;
        private DateTime m_NextPhaseShift;
        private DateTime m_NextDisruptWave;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public FlickeringRemnant()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a flickering remnant";
            Body = 58;
            BaseSoundID = 466;
            Hue = 1150; // ethereal shimmer

            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(400, 500);

            SetHits(550, 700);
            SetMana(1000);
            SetStam(500);

            SetDamage(15, 25);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 20, 35);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 60;

            AddItem(new LightSource()); // eerie glow

            m_AbilitiesInitialized = false;
        }

        public FlickeringRemnant(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null) return;

            if (!m_AbilitiesInitialized)
            {
                var rand = new Random();
                m_NextPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                m_NextDisruptWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 50));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextPulse)
                TemporalPulse();

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();

            if (DateTime.UtcNow >= m_NextDisruptWave)
                DisruptionWave();
        }

        private void TemporalPulse()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*The Flickering Remnant pulses with unstable energy*");
            PlaySound(0x5C3); // energy zap sound
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, Hue, 0);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 100); // pure energy
                    if (m is Mobile mob)
                    {
                        mob.SendMessage("You feel your energy being drained by a pulse of flickering magic!");
                        mob.Stam -= Utility.RandomMinMax(10, 20);
                        mob.Mana -= Utility.RandomMinMax(15, 25);
                    }
                }
            }

            m_NextPulse = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*The Remnant vanishes momentarily...*");
            PlaySound(0x1FE); // ghost vanish

            Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
            {
                if (!Deleted)
                {
                    Hidden = false;
                    PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*...and reappears behind its prey!*");
                    PlaySound(0x20C);
                }
            });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        private void DisruptionWave()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*The air distorts as a wave of broken time surges outward!*");
            PlaySound(0x5AB); // big disruption

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10, Hue, 0);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 50, 0, 0, 50);
                    m.FixedParticles(0x374A, 10, 30, 5013, Hue, 0, EffectLayer.CenterFeet);
                    if (m is Mobile mob)
                    {
                        mob.SendMessage("Your vision blurs as time itself turns against you!");
                        mob.Paralyze(TimeSpan.FromSeconds(3));
                    }
                }
            }

            m_NextDisruptWave = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 4);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new LightOfTheRemnant()); // rare drop
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
        }
    }

    public class LightOfTheRemnant : Item
    {
        public LightOfTheRemnant() : base(0x1F18)
        {
            Name = "Light of the Remnant";
            Hue = 1150;
            LootType = LootType.Blessed;
            Weight = 1.0;
        }

        public LightOfTheRemnant(Serial serial) : base(serial)
        {
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
        }
    }
}
