using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an inferno warden corpse")]
    public class InfernoWarden : BaseCreature
    {
        private DateTime m_NextFlamingShield;
        private DateTime m_NextBlazingCharge;
        private DateTime m_NextInfernoAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernoWarden()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an inferno warden";
            Body = 15; // Fire Elemental body
            BaseSoundID = 838;
			Hue = 1658; // Unique hue, reddish with molten effects

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

        public InfernoWarden(Serial serial)
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
                    m_NextFlamingShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBlazingCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextInfernoAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFlamingShield)
                {
                    ActivateFlamingShield();
                }

                if (DateTime.UtcNow >= m_NextBlazingCharge)
                {
                    PerformBlazingCharge();
                }

                if (DateTime.UtcNow >= m_NextInfernoAura)
                {
                    CreateInfernoAura();
                }
            }
        }

        private void ActivateFlamingShield()
        {
            // Damage nearby attackers with fire damage
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Player && m.Alive)
                {
                    m.SendMessage("You are scorched by the infernal heat!");
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }

            m_NextFlamingShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown for FlamingShield
        }

        private void PerformBlazingCharge()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    this.MoveToWorld(target.Location, target.Map);
                    target.SendMessage("The inferno warden charges at you, engulfing you in flames!");
                    target.Damage(Utility.RandomMinMax(20, 30), this);
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                    target.SendMessage("You are set ablaze by the inferno warden!");

                    Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() =>
                    {
                        if (target != null && !target.Deleted)
                            target.SendMessage("The inferno warden's flames fade away.");
                    }));

                    m_NextBlazingCharge = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set cooldown for BlazingCharge
                }
            }
        }

        private void CreateInfernoAura()
        {
            // Apply an aura effect that damages nearby players over time
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && m.Alive)
                {
                    m.SendMessage("The intense heat from the inferno warden burns you!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            m_NextInfernoAura = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown for InfernoAura
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
