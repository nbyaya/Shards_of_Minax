using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network; // Needed for MessageType and SendMessage

namespace Server.Mobiles
{
    [CorpseName("a spectral charger corpse")]
    public class SpectralCharger : BaseCreature
    {
        private DateTime m_NextBlinkStrike;
        private DateTime m_NextWail;
        private DateTime m_NextPhantomRush;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SpectralCharger()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Spectral Charger";
            Body = 101; // Same as centaur
            BaseSoundID = 679;
            Hue = 1150; // Ghostly hue (ethereal blue)

            SetStr(850, 1050);
            SetDex(180, 250);
            SetInt(250, 300);

            SetHits(900, 1200);
            SetStam(300);
            SetMana(600);

            SetDamage(24, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 65, 80);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.Magery, 90.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.MagicResist, 110.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;

            AddItem(new Longsword()); // Glowing ghost blade
            PackItem(new Arrow(Utility.RandomMinMax(90, 120)));

            m_AbilitiesInitialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextBlinkStrike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));
                    m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextPhantomRush = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextBlinkStrike)
                    BlinkStrike();

                if (DateTime.UtcNow >= m_NextWail)
                    SpectralWail();

                if (DateTime.UtcNow >= m_NextPhantomRush)
                    PhantomRush();
            }
        }

        private void BlinkStrike()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Spectral Charger vanishes into mist... *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5042, (int)EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromMilliseconds(800), () =>
            {
                if (Combatant == null || Deleted)
                    return;

                MoveToWorld(Combatant.Location, Map);
                PlaySound(0x20E);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* ...and strikes from behind! *");
                Combatant.Damage(Utility.RandomMinMax(20, 40), this);
            });

            m_NextBlinkStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SpectralWail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Spectral Charger releases a soul-rending wail! *");
            PlaySound(0x1F9); // Wail sound

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m.Player)
                {
                    m.FixedParticles(0x375A, 10, 15, 5036, EffectLayer.Head);
                    m.SendMessage(0x22, "Your soul trembles from the wail!");
                    m.Damage(Utility.RandomMinMax(15, 30), this);
                    m.Paralyze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void PhantomRush()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Spectral Charger bolts forward in a blur of speed! *");
            PlaySound(0x64F);

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 300), () =>
                {
                    if (Combatant is Mobile m && m.Alive)
                    {
                        m.Damage(Utility.RandomMinMax(10, 20), this);
                        m.SendMessage("You are struck by a blur of spectral force!");
                        m.Freeze(TimeSpan.FromSeconds(0.5));
                    }
                });
            }

            m_NextPhantomRush = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                PackItem(new SpectralShard());
            }
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 4;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public SpectralCharger(Serial serial) : base(serial) { }

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

    public class SpectralShard : Item
    {
        [Constructable]
        public SpectralShard() : base(0x1F19) // Crystal-like appearance
        {
            Name = "a spectral shard";
            Hue = 1150;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public SpectralShard(Serial serial) : base(serial) { }

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
