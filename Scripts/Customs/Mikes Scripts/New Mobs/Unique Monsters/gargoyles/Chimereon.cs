using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a chimereon corpse")]
    public class Chimereon : BaseCreature
    {
        private DateTime m_NextMimicry;
        private DateTime m_NextChameleonSkin;
        private DateTime m_NextMirrorImageC;
        private DateTime m_NextAuraEffect;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private bool m_IsInvisible;

        [Constructable]
        public Chimereon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Chimereon the Shapeshifter";
            Body = 4; // Gargoyle body
            Hue = 1756; // Unique hue
			BaseSoundID = 372;

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

        public Chimereon(Serial serial)
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
                    m_NextMimicry = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextChameleonSkin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextMirrorImageC = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextAuraEffect = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMimicry)
                {
                    UseMimicry();
                }

                if (DateTime.UtcNow >= m_NextChameleonSkin)
                {
                    UseChameleonSkin();
                }

                if (DateTime.UtcNow >= m_NextMirrorImageC)
                {
                    UseMirrorImageC();
                }

                if (DateTime.UtcNow >= m_NextAuraEffect)
                {
                    ApplyAuraEffect();
                }
            }
        }

        private void UseMimicry()
        {
            Mobile target = FindRandomCreature();
            if (target != null)
            {
                Body = target.Body;
                Hue = target.Hue;

                // Randomly grant additional special ability or resistances based on the mimicked creature
                if (Utility.RandomBool())
                {
                    // Example: Temporarily gain poison resistance
                    SetResistance(ResistanceType.Poison, 60, 70);
                }
                else
                {
                    // Example: Temporarily deal more damage
                    SetDamage(30, 40);
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Chimereon mimics a nearby creature! *");

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    Body = 4; // Revert to original body
                    Hue = 1150; // Revert to original hue
                    SetDamage(25, 35); // Revert to original damage range
                    SetResistance(ResistanceType.Poison, 30, 40); // Revert to original poison resistance
                });

                m_NextMimicry = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        private Mobile FindRandomCreature()
        {
            foreach (Mobile mob in GetMobilesInRange(10))
            {
                if (mob is BaseCreature && mob != this)
                    return mob;
            }

            return null;
        }

        private void UseChameleonSkin()
        {
            if (!m_IsInvisible)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Chimereon blends into its surroundings! *");
                this.Hidden = true;
                m_IsInvisible = true;
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    this.Hidden = false;
                    m_IsInvisible = false;
                });

                m_NextChameleonSkin = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void UseMirrorImageC()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Chimereon creates illusory copies of itself! *");

            for (int i = 0; i < 3; i++)
            {
                MirrorImageC clone = new MirrorImageC(this);
                clone.MoveToWorld(new Point3D(X + Utility.RandomMinMax(-3, 3), Y + Utility.RandomMinMax(-3, 3), Z), Map);
            }

            m_NextMirrorImageC = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }

        private void ApplyAuraEffect()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Chimereon radiates an aura of chaos! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    // Apply an area-of-effect damage or status effect
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, this);
                    m.SendMessage("The chaotic aura of Chimereon burns you!");
                }
            }

            m_NextAuraEffect = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

    public class MirrorImageC : BaseCreature
    {
        private Mobile m_Master;

        public MirrorImageC(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);
            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public MirrorImageC(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted || Combatant != null)
            {
                Delete();
                return;
            }

            Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
