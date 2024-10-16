using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an abyssal bouncer corpse")]
    public class AbyssalBouncer : BaseCreature
    {
        private DateTime m_NextVoidSurge;
        private DateTime m_NextAbyssalGrip;
        private DateTime m_NextPhantomJump;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public AbyssalBouncer()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an abyssal bouncer";
            Body = 205; // Rabbit body
            Hue = 2261; // Unique hue, dark and void-like

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

        public AbyssalBouncer(Serial serial)
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
                    m_NextVoidSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextAbyssalGrip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPhantomJump = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextVoidSurge)
                {
                    VoidSurge();
                }

                if (DateTime.UtcNow >= m_NextAbyssalGrip)
                {
                    AbyssalGrip();
                }

                if (DateTime.UtcNow >= m_NextPhantomJump)
                {
                    PhantomJump();
                }
            }
        }

        private void VoidSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Abyssal Bouncer unleashes a powerful shockwave of dark energy! *");
            PlaySound(0x1D7); // Dark energy sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(25, 35), 0, 0, 100, 0, 0); // Dark energy damage
                    m.SendMessage("You are hit by a dark energy shockwave and are knocked back!");
                    m.MovingParticles(this, 0x374A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0); // Dark particles
                    m.Location = new Point3D(m.X + Utility.Random(3, 6), m.Y + Utility.Random(3, 6), m.Z); // Knockback effect
                }
            }

            m_NextVoidSurge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void AbyssalGrip()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Abyssal Bouncer's grip pulls you into the abyss, draining your strength! *");
                PlaySound(0x1D8); // Gripping sound

                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    target.MoveToWorld(this.Location, this.Map);
                    target.SendMessage("You are pulled closer by a chilling grip and feel your strength fading!");

                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (target != null && target.Alive)
                        {
                            AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0); // Damage over time
                            target.SendMessage("You suffer from the abyssal grip's lingering effects!");
                            target.Damage(Utility.RandomMinMax(10, 20)); // Additional damage over time
                        }
                    });

                    target.SendMessage("Your strength is drained by the abyssal grip!");
                    target.SendMessage("You feel a weakening debuff!");
                    target.Dex -= 10; // Apply debuff

                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => target.Dex += 10); // Remove debuff after 5 seconds
                }

                m_NextAbyssalGrip = DateTime.UtcNow + TimeSpan.FromSeconds(40);
            }
        }

        private void PhantomJump()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Abyssal Bouncer creates confusing illusions of itself! *");
            PlaySound(0x1D9); // Illusion sound

            for (int i = 0; i < 4; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 500), () =>
                {
                    if (Combatant != null && !Deleted)
                    {
                        Mobile phantom = new AbyssalBouncerPhantom();
                        phantom.Location = this.Location;
                        phantom.Map = this.Map;
                        phantom.Direction = (Direction)Utility.Random(8);
                        // No need to call OnThink on the phantom
                    }
                });
            }

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m != this && m.Alive && CanBeHarmful(m))
                    {
                        m.SendMessage("You are confused by the illusions created by the Abyssal Bouncer!");
                        m.Paralyze(TimeSpan.FromSeconds(2)); // Temporary confusion effect
                    }
                }
            });

            m_NextPhantomJump = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

    public class AbyssalBouncerPhantom : BaseCreature
    {
        [Constructable]
        public AbyssalBouncerPhantom()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an abyssal phantom";
            Body = 205; // Same body as the main creature
            Hue = 1152; // Slightly different hue for illusion

            SetStr(50);
            SetDex(80);
            SetInt(50);

            SetHits(1);
            SetMana(0);

            SetDamage(0, 0);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 0);

            SetResistance(ResistanceType.Physical, 0);
            SetResistance(ResistanceType.Fire, 0);
            SetResistance(ResistanceType.Cold, 0);
            SetResistance(ResistanceType.Poison, 0);
            SetResistance(ResistanceType.Energy, 0);

            Fame = 0;
            Karma = 0;

            Tamable = false;
            Hidden = true;
        }

        public AbyssalBouncerPhantom(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            if (DateTime.UtcNow >= DateTime.UtcNow + TimeSpan.FromSeconds(1))
            {
                Delete();
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
