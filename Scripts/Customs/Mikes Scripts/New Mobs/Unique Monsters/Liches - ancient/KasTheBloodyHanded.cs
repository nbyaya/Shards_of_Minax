using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Kas's corpse")]
    public class KasTheBloodyHanded : BaseCreature
    {
        private DateTime m_NextVampiricStrike;
        private DateTime m_NextBloodFrenzy;
        private DateTime m_NextBloodWave;
        private bool m_BloodFrenzyActive;
        private bool m_BloodWaveActive;

        [Constructable]
        public KasTheBloodyHanded()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Kas the Bloody-Handed";
            Body = 78; // Ancient Lich body
            Hue = 1359; // Blood-red hue
			BaseSoundID = 412;

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
            PackNecroReg(100, 200);
        }

        public KasTheBloodyHanded(Serial serial)
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
                if (Hits < HitsMax * 0.5 && !m_BloodFrenzyActive)
                {
                    ActivateBloodFrenzy();
                }

                if (DateTime.UtcNow >= m_NextVampiricStrike)
                {
                    VampiricStrike();
                }

                if (DateTime.UtcNow >= m_NextBloodFrenzy && !m_BloodFrenzyActive)
                {
                    BloodFrenzy();
                }

                if (DateTime.UtcNow >= m_NextBloodWave)
                {
                    BloodWave();
                }
            }
        }

        private void VampiricStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kas performs a Vampiric Strike! *");
            PlaySound(0x1D2); // Custom sound

            if (Combatant != null)
            {
                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

                // Heal Kas for half of the damage dealt
                Hits += damage / 2;
                Hits = Math.Min(Hits, HitsMax);
            }

            m_NextVampiricStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void BloodFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kas enters a Blood Frenzy! *");
            PlaySound(0x1D3); // Custom sound

            m_BloodFrenzyActive = true;
            SetDamage(25, 35); // Increased damage

            m_NextBloodFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown
        }

        private void BloodWave()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kas unleashes a wave of blood! *");
            PlaySound(0x1D4); // Custom sound

            // Create a visual blood wave effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10); // Blood splash effect

            // Deal damage to all mobiles in a radius
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by a wave of blood!");
                }
            }

            m_NextBloodWave = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown
        }

        private void ActivateBloodFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kas’s bloodthirst reaches a new height! *");
            PlaySound(0x1D3); // Custom sound

            // Visual blood aura effect
            FixedParticles(0x36D4, 9, 32, 5030, EffectLayer.Waist);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (Hits < HitsMax * 0.5 && !m_BloodFrenzyActive)
            {
                ActivateBloodFrenzy();
            }
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
