using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a wailing sprite corpse")]
    public class WailingSprite : BaseCreature
    {
        private DateTime m_NextWail;
        private DateTime m_NextPhantomBlink;
        private DateTime m_NextDreamDrain;
        private bool m_InitializedAbilities;

        [Constructable]
        public WailingSprite()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Wailing Sprite";
            Body = 128; // Pixie body
            Hue = 1153; // Ethereal bluish-violet glow
            BaseSoundID = 0x467;

            SetStr(150, 200);
            SetDex(301, 400);
            SetInt(401, 500);

            SetHits(400, 550);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 50);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 45, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 65, 75);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
        }

        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_InitializedAbilities)
                {
                    m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    m_NextPhantomBlink = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                    m_NextDreamDrain = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                    m_InitializedAbilities = true;
                }

                if (DateTime.UtcNow >= m_NextWail)
                    UnholyWail();

                if (DateTime.UtcNow >= m_NextPhantomBlink)
                    PhantomBlink();

                if (DateTime.UtcNow >= m_NextDreamDrain)
                    DreamDrain();
            }
        }

        private void UnholyWail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* emits a chilling wail that tears at the soul *");
            PlaySound(0x667); // Wail sound

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    if (m is Mobile target)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 100, 0);
                        target.FixedParticles(0x37B9, 10, 20, 5032, EffectLayer.Head);
                        target.SendMessage(38, "Your ears ring and your thoughts blur as the sprite's wail pierces your mind!");

                        // Randomly reduce one attribute temporarily
                        int debuff = Utility.RandomMinMax(10, 20);
                        target.Dex -= debuff;

                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => target.Dex += debuff);
                    }
                }
            }

            m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void PhantomBlink()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* vanishes into shimmering mist *");
            PlaySound(0x20E); // Blink sound
            Effects.SendLocationEffect(Location, Map, 0x3728, 13);

            // Blink to random nearby location
            int xOffset = Utility.RandomMinMax(-5, 5);
            int yOffset = Utility.RandomMinMax(-5, 5);
            Point3D newLoc = new Point3D(X + xOffset, Y + yOffset, Z);

            if (Map.CanFit(newLoc, 2))
            {
                MoveToWorld(newLoc, Map);
                Effects.SendLocationEffect(newLoc, Map, 0x3728, 13);
            }

            m_NextPhantomBlink = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void DreamDrain()
        {
            if (Combatant != null && Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* whispers forgotten memories into the wind *");
                PlaySound(0x5C9); // Whisper-like sound

                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 25, 25, 50);
                target.Mana -= Utility.RandomMinMax(15, 30);
                target.Stam -= Utility.RandomMinMax(10, 20);

                target.SendMessage(68, "You feel your dreams unravel and your strength fade...");

                m_NextDreamDrain = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, 5);
            PackItem(new EtherealDust()); // Custom loot drop
        }

        public override int Meat => 1;
        public override int Hides => 5;
        public override HideType HideType => HideType.Spined;

        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.2)
                c.DropItem(new SpiritTear()); // Rare drop
        }

        public WailingSprite(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_InitializedAbilities = false;
        }
    }

    public class EtherealDust : Item
    {
        [Constructable]
        public EtherealDust() : base(0x573B)
        {
            Name = "Ethereal Dust";
            Hue = 1153;
            Weight = 1.0;
        }

        public EtherealDust(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SpiritTear : Item
    {
        [Constructable]
        public SpiritTear() : base(0x1F09)
        {
            Name = "Tear of the Spirit Realm";
            Hue = 1260;
            Weight = 0.1;
        }

        public SpiritTear(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
