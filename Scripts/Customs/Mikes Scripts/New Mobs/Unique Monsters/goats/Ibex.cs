using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server;

namespace Server.Mobiles
{
    [CorpseName("an ibex corpse")]
    public class Ibex : BaseCreature
    {
        private DateTime m_NextHornCharge;
        private DateTime m_NextAgileEvasion;
        private DateTime m_NextRoaringBlast;
        private DateTime m_NextStunningCharge;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public Ibex()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Ibex";
            Body = 0xD1; // Goat body
            Hue = 1909; // Light brown hue
			BaseSoundID = 0x99;

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

        public Ibex(Serial serial)
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
                    m_NextHornCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAgileEvasion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRoaringBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextStunningCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHornCharge)
                {
                    HornCharge();
                }

                if (DateTime.UtcNow >= m_NextAgileEvasion)
                {
                    AgileEvasion();
                }

                if (DateTime.UtcNow >= m_NextRoaringBlast)
                {
                    RoaringBlast();
                }

                if (DateTime.UtcNow >= m_NextStunningCharge)
                {
                    StunningCharge();
                }
            }
        }

        private void HornCharge()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ibex charges with its mighty horns!*");
                Combatant.PlaySound(0x4B0);
                AOS.Damage(Combatant, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                m_NextHornCharge = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for HornCharge
            }
        }

        private void AgileEvasion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ibex nimbly evades your attacks!*");
            this.Dex += 15; // Temporarily boost dexterity
            this.VirtualArmor += 15; // Increase dodge chance
            Timer.DelayCall(TimeSpan.FromSeconds(12), new TimerCallback(ResetAgileEvasion));
            m_NextAgileEvasion = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for AgileEvasion
        }

        private void ResetAgileEvasion()
        {
            this.Dex -= 15;
            this.VirtualArmor -= 15;
        }

        private void RoaringBlast()
        {
            if (Combatant != null && Combatant is Mobile mobileCombatant)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ibex lets out a deafening roar!*");
                // Use SendLocationEffect or another suitable method
                Effects.SendLocationEffect(Combatant.Location, Combatant.Map, 0x3B2, 10, 20);
                mobileCombatant.SendMessage("You are stunned by the Ibex's roar!");
                // If Freeze is not available, consider using other methods to immobilize
                mobileCombatant.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(() => mobileCombatant.Frozen = false));
                m_NextRoaringBlast = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for RoaringBlast
            }
        }

        private void StunningCharge()
        {
            if (Combatant != null && Combatant is Mobile mobileCombatant)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ibex charges with blinding speed!*");
                Combatant.PlaySound(0x4B0);
                AOS.Damage(Combatant, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                mobileCombatant.SendMessage("You are dazed by the Ibex's charge!");
                Combatant.FixedParticles(0x36BD, 10, 30, 5052, EffectLayer.LeftFoot);
                Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(() => mobileCombatant.SendMessage("You recover from the daze.")));
                m_NextStunningCharge = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for StunningCharge
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
