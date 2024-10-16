using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an ember axis corpse")]
    public class EmberAxis : BaseCreature
    {
        private DateTime m_NextFlameTrail;
        private DateTime m_NextInfernoCharge;
        private DateTime m_NextEmberBurst;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EmberAxis()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Ember Axis";
            Body = 0xEA; // GreatHart body
            Hue = 1981; // Fiery orange hue with embers

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

        public EmberAxis(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextFlameTrail = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextInfernoCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEmberBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFlameTrail)
                {
                    FlameTrail();
                }

                if (DateTime.UtcNow >= m_NextInfernoCharge)
                {
                    InfernoCharge();
                }

                if (DateTime.UtcNow >= m_NextEmberBurst)
                {
                    EmberBurst();
                }
            }
        }

        private void FlameTrail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ember Axis leaves a fiery path!*");

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (m.InRange(this, 1))
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 100, 0, 0, 0);
                            m.SendMessage("You walk through a burning trail!");
                        }
                    });
                }
            }

            m_NextFlameTrail = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Adjust the cooldown as needed
        }

        private void InfernoCharge()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ember Axis engulfs you in flames!*");
            Mobile mobile = Combatant as Mobile;
            if (mobile != null)
            {
                mobile.SendMessage("You are engulfed in flames!");
            }

            int damage = Utility.RandomMinMax(15, 25);
            AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                if (Combatant != null && Combatant.Alive)
                {
                    AOS.Damage(Combatant, this, damage / 2, 0, 100, 0, 0, 0);
                }
            });

            m_NextInfernoCharge = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Adjust the cooldown as needed
        }

        private void EmberBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ember Axis releases a burst of fiery embers!*");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("Fiery embers sting you!");
                }
            }

            m_NextEmberBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Adjust the cooldown as needed
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
