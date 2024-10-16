using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of King Arthur")]
    public class UltimateMasterChivalry : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterChivalry()
            : base(AIType.AI_Melee)
        {
            Name = "King Arthur";
            Title = "The Once and Future King";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(505, 625);
            SetDex(102, 150);
            SetInt(305, 425);

            SetHits(15000);
            SetMana(3000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Parry, 120.0);
            SetSkill(SkillName.Chivalry, 120.0);

            Fame = 30000;
            Karma = 30000;

            VirtualArmor = 80;

            AddItem(new PlateChest());
            AddItem(new PlateArms());
            AddItem(new PlateLegs());
            AddItem(new PlateHelm());
            AddItem(new PlateGloves());
            AddItem(new Boots());
            AddItem(new Cloak(0x59B));

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public UltimateMasterChivalry(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(Excalibur), typeof(HolyGrail) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ExcaliburReplica), typeof(GrailRelic) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(Excalibur), typeof(GrailRelic) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 8);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Chivalry, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ExcaliburReplica());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new GrailRelic());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: DivineStrike(defender); break;
                    case 1: KnightShield(); break;
                    case 2: CallOfCamelot(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void DivineStrike(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(80, 100);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                target.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                target.PlaySound(0x1FA);
            }
        }

        public void KnightShield()
        {
            this.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            this.PlaySound(0x1F7);
            this.VirtualArmor += 50;
            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RemoveKnightShield));
        }

        private void RemoveKnightShield()
        {
            this.VirtualArmor -= 50;
        }

        public void CallOfCamelot()
        {
            for (int i = 0; i < 3; i++)
            {
                BaseCreature knight = new ChampionKnight();
                knight.Team = this.Team;
                knight.MoveToWorld(this.Location, this.Map);
                knight.Combatant = this.Combatant;
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

    public class Excalibur : BaseWeapon
    {
        [Constructable]
        public Excalibur() : base(0xF5E)
        {
            Name = "Excalibur";
            Hue = 1157;
            WeaponAttributes.HitFireball = 50;
            WeaponAttributes.HitLightning = 50;
            WeaponAttributes.HitLowerAttack = 50;
            WeaponAttributes.HitLowerDefend = 50;
            Attributes.SpellChanneling = 1;
            Attributes.WeaponDamage = 75;
        }

        public Excalibur(Serial serial) : base(serial)
        {
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
        }
    }

    public class HolyGrail : Item
    {
        [Constructable]
        public HolyGrail() : base(0xE2D)
        {
            Name = "Holy Grail";
            Hue = 1174;
        }

        public HolyGrail(Serial serial) : base(serial)
        {
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
        }
    }
    
    public class ExcaliburReplica : BaseWeapon
    {
        [Constructable]
        public ExcaliburReplica() : base(0xF5E)
        {
            Name = "Replica of Excalibur";
            Hue = 1157;
            WeaponAttributes.HitFireball = 25;
            WeaponAttributes.HitLightning = 25;
            WeaponAttributes.HitLowerAttack = 25;
            WeaponAttributes.HitLowerDefend = 25;
            Attributes.SpellChanneling = 1;
            Attributes.WeaponDamage = 50;
        }

        public ExcaliburReplica(Serial serial) : base(serial)
        {
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
        }
    }

    public class GrailRelic : Item
    {
        [Constructable]
        public GrailRelic() : base(0xE2D)
        {
            Name = "Relic of the Grail";
            Hue = 1174;
        }

        public GrailRelic(Serial serial) : base(serial)
        {
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
        }
    }

    public class ChampionKnight : BaseCreature
    {
        [Constructable]
        public ChampionKnight()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Knight of Camelot";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(300, 400);
            SetDex(90, 120);
            SetInt(70, 90);

            SetHits(1000);
            SetMana(500);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Swords, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.Parry, 100.0);

            Fame = 10000;
            Karma = 10000;

            VirtualArmor = 50;

            AddItem(new PlateChest());
            AddItem(new PlateArms());
            AddItem(new PlateLegs());
            AddItem(new PlateHelm());
            AddItem(new PlateGloves());
            AddItem(new Boots());
            AddItem(new Longsword());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public ChampionKnight(Serial serial)
            : base(serial)
        {
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
