using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a forgotten warden corpse")]
    public class ForgottenWarden : BaseCreature
    {
        private DateTime m_NextAncientCurse;
        private DateTime m_NextSpectralBinding;
        private DateTime m_NextForgottenEchoes;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ForgottenWarden()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Forgotten Warden";
            Body = 205; // Jackrabbit body
            Hue = 2251; // Ghostly hue

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

            m_AbilitiesInitialized = false;
        }

        public ForgottenWarden(Serial serial)
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
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextAncientCurse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSpectralBinding = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextForgottenEchoes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextAncientCurse)
                {
                    AncientCurse();
                }

                if (DateTime.UtcNow >= m_NextSpectralBinding)
                {
                    SpectralBinding();
                }

                if (DateTime.UtcNow >= m_NextForgottenEchoes)
                {
                    ForgottenEchoes();
                }
            }
        }

        private void AncientCurse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Forgotten Warden’s curse haunts you through the ages! *");
            PlaySound(0x1F4); // Ghostly sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0); // Psychic damage
                    m.SendMessage("You are struck by the ancient curse, reducing your strength and making you more vulnerable!");
                    m.Str = (int)(m.Str * 0.5); // Reduce strength
                    m.Dex = (int)(m.Dex * 0.5); // Reduce dexterity
                    m.Int = (int)(m.Int * 0.5); // Reduce intelligence
                }
            }

            m_NextAncientCurse = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void SpectralBinding()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Spectral chains bind your soul! *");
            PlaySound(0x1F5); // Chains sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("Spectral chains bind and immobilize you!");
					m.Freeze(TimeSpan.FromSeconds(2)); 
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        if (m != null && m.Alive)
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0); // Psychic damage
                        }
                    });
                }
            }

            m_NextSpectralBinding = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ForgottenEchoes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Forgotten echoes disorient you! *");
            PlaySound(0x1F6); // Echo sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0); // Psychic damage
                    m.SendMessage("The echo of forgotten voices disorients you!");
                    m.Paralyzed = true; // Disorient for a short time
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        if (m != null && m.Alive)
                        {
                            m.Paralyzed = false;
                        }
                    });
                }
            }

            m_NextForgottenEchoes = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }
}
