using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an icy solen corpse")]
    public class IcySolenServant : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextMirrorClone;
        private DateTime m_NextFreezeTouch;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IcySolenServant()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Icy Solen Servant";
            Body = 787; // Solen Warrior body ID
            Hue = 1152; // Icy blue/white hue (unique)
            BaseSoundID = 959; // Solen click/screech sound

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(1200, 1600);

            SetDamage(18, 24);

            SetDamageType(ResistanceType.Cold, 75);
            SetDamageType(ResistanceType.Physical, 25);

            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 45, 60);

            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 75.0, 95.0);

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 80;

            m_AbilitiesInitialized = false;
        }

        public IcySolenServant(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 4;
        public override bool Unprovokable => true;
        public override bool BardImmune => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 4);
            AddLoot(LootPack.HighScrolls, 2);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    var rand = new Random();
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                    m_NextMirrorClone = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25));
                    m_NextFreezeTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextMirrorClone)
                    MirrorClone();

                if (DateTime.UtcNow >= m_NextFreezeTouch)
                    FreezeTouch();
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*An icy nova explodes outward!*");
            PlaySound(0x64C); // Ice burst sound
            Effects.PlaySound(Location, Map, 0x10B); // Magic frost sound
            Effects.SendLocationEffect(Location, Map, 0x376A, 20);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    if (m is Mobile target)
                    {
                        target.Frozen = true;
                        target.SendMessage(1152, "You are frozen in place by the icy explosion!");
                        Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                        {
                            if (target != null)
                                target.Frozen = false;
                        });
                    }
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void MirrorClone()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Icy Solen summons a reflection of itself!*");
            PlaySound(0x5A3); // Illusion sound

            BaseCreature clone = new IcySolenMirrorClone();
            clone.MoveToWorld(Location, Map);
            clone.Combatant = Combatant;

            m_NextMirrorClone = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void FreezeTouch()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Icy Solen touches you with paralyzing cold!*");
                PlaySound(0x14E);

                target.Paralyze(TimeSpan.FromSeconds(4));
                AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                target.SendMessage(1152, "Your limbs are seized by supernatural frost!");
            }

            m_NextFreezeTouch = DateTime.UtcNow + TimeSpan.FromSeconds(20);
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

    public class IcySolenMirrorClone : BaseCreature
    {
        [Constructable]
        public IcySolenMirrorClone()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Icy Solen Mirror";
            Body = 787;
            Hue = 1152;
            BaseSoundID = 959;

            SetStr(100);
            SetDex(100);
            SetInt(50);

            SetHits(100);
            SetDamage(10, 15);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 20);
            SetResistance(ResistanceType.Cold, 100);

            VirtualArmor = 20;

            Timer.DelayCall(TimeSpan.FromSeconds(15), () =>
            {
                if (!Deleted)
                    Delete();
            });
        }

        public IcySolenMirrorClone(Serial serial) : base(serial) { }

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
