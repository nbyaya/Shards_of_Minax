using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a shattered majesty's corpse")]
    public class ShatteredMajesty : BaseCreature
    {
        private DateTime m_NextReflection;
        private DateTime m_NextFrozenTime;
        private DateTime m_NextCrownOfIce;
        private DateTime m_NextShatterCurse;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ShatteredMajesty()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Shattered Majesty";
            Body = 252;
            BaseSoundID = 0x482;
            Hue = 1153; // Ghostly icy-blue

            SetStr(350, 400);
            SetDex(150, 180);
            SetInt(550, 600);

            SetHits(1600, 1800);
            SetMana(3000);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 80, 100);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Necromancy, 105.0, 120.0);
            SetSkill(SkillName.SpiritSpeak, 100.0, 115.0);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 90;
            m_AbilitiesInitialized = false;
        }

        public ShatteredMajesty(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 8);
        }

        public override int GetDeathSound() => 0x370;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_AbilitiesInitialized)
            {
                m_NextReflection = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
                m_NextFrozenTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
                m_NextCrownOfIce = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                m_NextShatterCurse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 75));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextReflection)
                FracturedReflection();

            if (DateTime.UtcNow >= m_NextFrozenTime)
                FrozenTime();

            if (DateTime.UtcNow >= m_NextCrownOfIce)
                CrownOfIce();

            if (DateTime.UtcNow >= m_NextShatterCurse)
                ShatterCurse();
        }

        private void FracturedReflection()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*Shattered Majesty splits into mirrored echoes!*");
            PlaySound(0x5C3); // Ethereal clone sound

            for (int i = 0; i < 2; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 500), () =>
                {
                    ShatteredReflection clone = new ShatteredReflection(this);
                    clone.MoveToWorld(this.Location, this.Map);
                });
            }

            m_NextReflection = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void FrozenTime()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*Time stills as frost creeps across the air...*");
            PlaySound(0x10B); // Freeze sound
            Effects.SendLocationEffect(Location, Map, 0x3728, 15);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.Hidden && m is Mobile mobile)
                {
                    mobile.Freeze(TimeSpan.FromSeconds(4));
                    mobile.SendMessage(0x480, "You are frozen in time!");
                }
            }

            m_NextFrozenTime = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void CrownOfIce()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Emote, 0x480, true, "*A crown of jagged frost erupts around " + target.Name + "!*");
                target.SendMessage(0x480, "You are struck by the Crown of Ice!");

                AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 100, 0, 0, 0);
                target.Mana -= Utility.RandomMinMax(10, 30);
                Effects.SendLocationEffect(target.Location, target.Map, 0x374A, 30);
                target.PlaySound(0x64C); // Chilling crack

                m_NextCrownOfIce = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void ShatterCurse()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, true, "*A wave of cursed frost shatters across the battlefield!*");
            PlaySound(0x5C9); // Curse sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is Mobile mobile)
                {
                    AOS.Damage(mobile, this, Utility.RandomMinMax(20, 30), 0, 80, 0, 20, 0);
                    mobile.FixedParticles(0x376A, 9, 20, 5032, EffectLayer.Waist);
                    mobile.SendMessage(0x22, "Your resistances shatter under the cursed frost!");

                }
            }

            m_NextShatterCurse = DateTime.UtcNow + TimeSpan.FromSeconds(75);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class ShatteredReflection : BaseCreature
    {
        private Timer m_Timer;

        public ShatteredReflection(ShatteredMajesty origin)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an echo of majesty";
            Body = 252;
            BaseSoundID = 0x482;
            Hue = 1153;
            SetStr(50);
            SetDex(50);
            SetInt(50);
            SetHits(50);
            SetDamage(5, 10);

            VirtualArmor = 10;
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(8), Delete);
        }

        public ShatteredReflection(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true;
        public override bool DeleteOnRelease => true;

        public override void OnThink()
        {
            base.OnThink();
            if (Combatant == null)
                return;
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
