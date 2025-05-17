using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a frostwoven corpse")]
    public class FrostwovenMage : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextMirrorClone;
        private DateTime m_NextTimeSlip;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostwovenMage()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Frostwoven Mage";
            Body = 0x8F;
            BaseSoundID = 437;
            Hue = 1150; // Frosty, crystalline blue

            SetStr(200, 250);
            SetDex(110, 130);
            SetInt(300, 350);

            SetHits(550, 750);

            SetDamage(15, 22);
            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 95.0, 110.0);
            SetSkill(SkillName.Magery, 95.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 75.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 85.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            m_AbilitiesInitialized = false;
        }

        public FrostwovenMage(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextMirrorClone = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                    m_NextTimeSlip = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextMirrorClone)
                    MirrorClone();

                if (DateTime.UtcNow >= m_NextTimeSlip)
                    TimeSlip();
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*A wave of freezing power explodes outward!*");
            PlaySound(0x10B); // Frost explosion sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m is PlayerMobile)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 0, 100, 0, 0);
                    m.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => { m.Frozen = false; });
                    m.SendMessage(0x480, "You are frozen in place by the Frostwoven's power!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }

        private void MirrorClone()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47F, true, "*The Frostwoven splits into shimmering reflections!*");
            PlaySound(0x208);

            for (int i = 0; i < 2; i++)
            {
                BaseCreature clone = new FrostwovenClone();
                clone.Team = this.Team;
                clone.Combatant = this.Combatant;
                clone.MoveToWorld(this.Location, this.Map);
            }

            m_NextMirrorClone = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
        }

        private void TimeSlip()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47F, true, "*Time bends around the Frostwoven Mage!*");
            PlaySound(0x1F3); // Echoing shimmer

            // Teleport to random nearby location
            Point3D newLoc = new Point3D(this.X + Utility.RandomMinMax(-5, 5), this.Y + Utility.RandomMinMax(-5, 5), this.Z);
            this.MoveToWorld(newLoc, this.Map);

            if (Combatant is Mobile target)
            {
                target.SendMessage(0x480, "The Frostwoven Mage vanishes and reappears behind you!");
            }

            m_NextTimeSlip = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 3);

            if (Utility.RandomDouble() < 0.03) // 3% chance
            {
                PackItem(new GlacialFocus()); // Custom frost relic
            }
        }

        public override int TreasureMapLevel => 4;
        public override bool CanRummageCorpses => true;

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

    public class FrostwovenClone : BaseCreature
    {
        public FrostwovenClone()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shimmering reflection";
            Body = 0x8F;
            Hue = 1152; // Paler icy hue
            BaseSoundID = 437;

            SetStr(50);
            SetDex(80);
            SetInt(200);

            SetHits(100);
            SetDamage(5, 10);

            SetSkill(SkillName.Magery, 60.0);
            SetSkill(SkillName.MagicResist, 40.0);

            VirtualArmor = 20;

            Timer.DelayCall(TimeSpan.FromSeconds(15), Delete); // Self-destruct after time
        }

        public FrostwovenClone(Serial serial) : base(serial) { }

        public override void GenerateLoot() { } // No loot

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GlacialFocus : Item
    {
        public GlacialFocus() : base(0x1F2B)
        {
            Name = "Glacial Focus";
            Hue = 1150;
            Weight = 1.0;
        }

        public GlacialFocus(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
