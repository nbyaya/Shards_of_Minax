using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a luchador llama corpse")]
    public class LuchadorLlama : BaseCreature
    {
        private DateTime m_NextDropkick;
        private DateTime m_NextSuplex;
        private DateTime m_NextTaunt;
        private bool m_CrowdRoarActive;

        [Constructable]
        public LuchadorLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a luchador llama";
            Body = 0xDC; // Llama body
            Hue = 2154; // Unique hue for Luchador Llama
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

            m_CrowdRoarActive = false;
        }

        public LuchadorLlama(Serial serial)
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

            if (Combatant is Mobile combatant)
            {
                if (Hits <= HitsMax * 0.3 && !m_CrowdRoarActive)
                {
                    CrowdRoar();
                }

                if (DateTime.UtcNow >= m_NextDropkick)
                {
                    PerformDropkick(combatant);
                }

                if (DateTime.UtcNow >= m_NextSuplex)
                {
                    PerformSuplex(combatant);
                }

                if (DateTime.UtcNow >= m_NextTaunt)
                {
                    LuchaTaunt(combatant);
                }
            }
        }

        private void PerformDropkick(Mobile combatant)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Luchador Llama performs a flying dropkick! *");
            PlaySound(0x2D8); // Dropkick sound

            AOS.Damage(combatant, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
            combatant.SendMessage("You are hit by a powerful dropkick and knocked down!");

            // Knockback and knockdown effect
            combatant.MoveToWorld(new Point3D(combatant.X + Utility.RandomMinMax(-3, 3), combatant.Y + Utility.RandomMinMax(-3, 3), combatant.Z), combatant.Map);
            combatant.SendMessage("You are knocked down by the dropkick!");
            Timer.DelayCall(TimeSpan.FromSeconds(1), () => { if (combatant != null) combatant.Frozen = false; });
            combatant.Frozen = true;

            m_NextDropkick = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Dropkick
        }

        private void PerformSuplex(Mobile combatant)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Luchador Llama executes a stunning suplex! *");
            PlaySound(0x2D9); // Suplex sound

            AOS.Damage(combatant, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
            combatant.SendMessage("You are slammed to the ground and stunned!");

            // Apply stun and defense reduction
            combatant.Frozen = true;
            Timer.DelayCall(TimeSpan.FromSeconds(3), () => { if (combatant != null) combatant.Frozen = false; });
            combatant.VirtualArmor -= 10; // Temporary defense reduction

            m_NextSuplex = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Suplex
        }

        private void CrowdRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Luchador Llama roars with renewed strength! *");
            PlaySound(0x2D7); // Roar sound

            // Temporary strength, damage boost and damage reflection
            SetDamage(20, 35);
            VirtualArmor = 70;

            m_CrowdRoarActive = true;

            // Revert after 30 seconds
            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                SetDamage(12, 22);
                VirtualArmor = 50;
                m_CrowdRoarActive = false;
            });
        }

        private void LuchaTaunt(Mobile combatant)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Luchador Llama taunts you! *");
            PlaySound(0x2DA); // Taunt sound

            // Lower combatant's attack and defense
            combatant.SendMessage("The Luchador Llama's taunt lowers your attack and defense!");
            combatant.Damage(0, this); // Optional: Add a small amount of damage if needed
            combatant.VirtualArmor -= 5;
            combatant.Skills[SkillName.Tactics].Base -= 10;
            combatant.Skills[SkillName.Wrestling].Base -= 10;

            m_NextTaunt = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Taunt
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (m_CrowdRoarActive && attacker is Mobile mobileAttacker)
            {
                AOS.Damage(mobileAttacker, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0);
                mobileAttacker.SendMessage("You are hit by the Luchador Llama's counterattack!");
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
