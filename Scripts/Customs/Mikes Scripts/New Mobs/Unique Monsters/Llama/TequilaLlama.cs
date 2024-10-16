using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a tequila llama corpse")]
    public class TequilaLlama : BaseCreature
    {
        private DateTime m_NextTequilaBreath;
        private DateTime m_NextLiquidCourage;
        private bool m_IsInvulnerable;
        private bool m_IsConfusing;

        [Constructable]
        public TequilaLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Tequila Llama";
            Body = 0xDC; // Llama body
            Hue = 2149; // Unique hue for Tequila Llama
			this.BaseSoundID = 0x3F3;

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

            m_NextTequilaBreath = DateTime.UtcNow;
            m_NextLiquidCourage = DateTime.UtcNow;
            m_IsInvulnerable = false;
            m_IsConfusing = false;
        }

        public TequilaLlama(Serial serial)
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
                if (DateTime.UtcNow >= m_NextTequilaBreath)
                {
                    TequilaBreath();
                }

                if (DateTime.UtcNow >= m_NextLiquidCourage)
                {
                    LiquidCourage();
                }

                if (Hits < (HitsMax / 4) && !m_IsInvulnerable)
                {
                    ActivateLiquidCourage();
                }

                if (m_IsConfusing)
                {
                    ApplyConfusion();
                }
            }
        }

        private void TequilaBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tequila Llama exhales a cloud of intoxicating fumes! *");
            PlaySound(0x208); // Unique sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Periodic damage
                    m.SendMessage("You feel disoriented from the llama's breath!");
                    m.AddToBackpack(new Item(0x14F0)); // Add a debuff or effect here
                }
            }

            m_NextTequilaBreath = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void LiquidCourage()
        {
            if (Hits < (HitsMax / 4))
            {
                ActivateLiquidCourage();
            }

            m_NextLiquidCourage = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Liquid Courage
        }

        private void ActivateLiquidCourage()
        {
            if (m_IsInvulnerable)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tequila Llama gains a surge of courage and becomes temporarily invulnerable! *");
            PlaySound(0x208); // Unique sound

            // Increase damage and set invulnerability
            this.SetDamage(15, 25);
            this.VirtualArmor = 50;
            m_IsInvulnerable = true;

            // Add visual effect
            FixedParticles(0x373A, 9, 32, 5030, EffectLayer.Waist);
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => DeactivateLiquidCourage());
        }

        private void DeactivateLiquidCourage()
        {
            if (m_IsInvulnerable)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tequila Llama's courage fades away. *");
                this.SetDamage(8, 15);
                this.VirtualArmor = 30;
                m_IsInvulnerable = false;
            }
        }

        private void ApplyConfusion()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("The tequila fumes confuse you, causing your attacks to miss!");
                    m.SendMessage("You are disoriented!");
                    // Apply confusion effect (e.g., reduce accuracy or chance to hit)
                    // Example: m.AddToBackpack(new Item(0x14F0)); // Add a confusion debuff item or effect
                }
            }
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_IsInvulnerable)
            {
                damage = 0; // No melee damage when invulnerable
                from.SendMessage("Your attack is deflected by the llama's courage!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_IsInvulnerable);
            writer.Write(m_IsConfusing);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsInvulnerable = reader.ReadBool();
            m_IsConfusing = reader.ReadBool();
        }
    }
}
