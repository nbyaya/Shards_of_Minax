using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Hattori Hanzō")]
    public class UltimateMasterNinjitsu : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterNinjitsu()
            : base(AIType.AI_Melee)
        {
            Name = "Hattori Hanzō";
            Title = "The Ultimate Ninja Master";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(350, 475);
            SetDex(600, 750);
            SetInt(150, 200);

            SetHits(15000);
            SetStam(5000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Ninjitsu, 120.0);
            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 75;
			
            AddItem(new NinjaTabi());
            AddItem(new LeatherNinjaPants());
            AddItem(new LeatherNinjaJacket());
            AddItem(new LeatherNinjaMitts());
            AddItem(new LeatherNinjaHood());
        }

        public UltimateMasterNinjitsu(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(NinjaTo), typeof(ShurikenPouch) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(NinjaTo), typeof(ShurikenPouch) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(NinjaTo), typeof(ShurikenPouch) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Ninjitsu, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new NinjaTo());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ShurikenPouch());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: ShadowStep(defender); break;
                    case 1: SmokeBomb(); break;
                    case 2: Assassinate(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void ShadowStep(Mobile target)
        {
            if (target != null)
            {
                this.Location = target.Location;
                this.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Waist);
                this.PlaySound(0x208);
            }
        }

        public void SmokeBomb()
        {
            this.FixedParticles(0x3735, 1, 30, 9502, EffectLayer.Waist);
            this.PlaySound(0x228);
            this.Hidden = true;
        }

        public void Assassinate(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                target.PlaySound(0x207);
                AOS.Damage(target, this, Utility.RandomMinMax(80, 100), 100, 0, 0, 0, 0);
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
    
    public class NinjaTo : Item
    {
        [Constructable]
        public NinjaTo() : base(0x27A9)
        {
            Name = "Ninja-to";
            Weight = 5.0;
            Hue = 0x66D;
        }

        public NinjaTo(Serial serial) : base(serial)
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
    
    public class ShurikenPouch : Item
    {
        [Constructable]
        public ShurikenPouch() : base(0x10B1)
        {
            Name = "Shuriken Pouch";
            Weight = 1.0;
            Hue = 0x66D;
        }

        public ShurikenPouch(Serial serial) : base(serial)
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
