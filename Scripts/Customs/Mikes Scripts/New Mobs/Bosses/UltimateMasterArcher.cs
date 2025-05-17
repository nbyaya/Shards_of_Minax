using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Robin Hood")]
    public class UltimateMasterArcher : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterArcher()
            : base(AIType.AI_Archer)
        {
            Name = "Robin Hood";
            Title = "The Prince of Thieves";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Archery, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Healing, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Bow());
            AddItem(new Boots(0x1BB));
            AddItem(new Shirt(0x6D1));
            AddItem(new ShortPants(0x1BB));
            AddItem(new Cloak(0x59D));

            HairItemID = 0x2044; // Long Hair
            HairHue = 0x455;

            PackItem(new PowerScroll(SkillName.Archery, 200.0));
        }

        public UltimateMasterArcher(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(HoodsLongbow), typeof(QuiverOfTheGreenWood) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(PowerScroll) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { }; }
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

            c.DropItem(new PowerScroll(SkillName.Archery, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new HoodsLongbow());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new QuiverOfTheGreenWood());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: PiercingArrow(defender); break;
                    case 1: Volley(); break;
                    case 2: Evasion(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void PiercingArrow(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);

                int damage = Utility.RandomMinMax(70, 90);

                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                target.PlaySound(0x207);
            }
        }

        public void Volley()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(30, 50);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
            }
        }

        public void Evasion()
        {
            this.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            this.PlaySound(0x1FA);

            this.VirtualArmor += 50;

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(RemoveEvasion));
        }

        public void RemoveEvasion()
        {
            this.VirtualArmor -= 50;
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

    public class HoodsLongbow : Bow
    {
        [Constructable]
        public HoodsLongbow() : base(0x13B2)
        {
            Name = "Hood's Longbow";
            Hue = 0x59D;
            WeaponAttributes.HitLeechMana = 50;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 50;
        }
		
		        public override int EffectID
        {
            get
            {
                return 0x1BFE;
            }
        }
        
		public override Type AmmoType
        {
            get
            {
                return typeof(Bolt);
            }
        }
        public override Item Ammo
        {
            get
            {
                return new Bolt();
            }
        }
        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.ConcussionBlow;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.MortalStrike;
            }
        }

        public HoodsLongbow(Serial serial) : base(serial)
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

    public class QuiverOfTheGreenWood : BaseQuiver
    {
        [Constructable]
        public QuiverOfTheGreenWood() : base()
        {
            Name = "Quiver of the Green Wood";
            Hue = 0x59D;
            WeightReduction = 50;
            LowerAmmoCost = 100;
        }

        public QuiverOfTheGreenWood(Serial serial) : base(serial)
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
}
