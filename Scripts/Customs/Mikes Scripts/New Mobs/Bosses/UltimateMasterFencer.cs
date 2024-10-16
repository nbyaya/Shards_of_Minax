using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Zorro")]
    public class UltimateMasterFencer : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterFencer()
            : base(AIType.AI_Melee)
        {
            Name = "Zorro";
            Title = "The Masked Swordsman";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(400, 500);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(15000);
            SetMana(2000);

            SetDamage(30, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Healing, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 75;

            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
            AddItem(new BodySash(Utility.RandomNeutralHue()));
            AddItem(new LeatherGloves());
            AddItem(new FloppyHat());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x47E;

            Item mask = new Item(0x154B); // Zorro's Mask
            mask.Hue = 0x47E;
            mask.Movable = false;
            AddItem(mask);
        }

        public UltimateMasterFencer(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ZorrosRapier), typeof(MaskOfZorro) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ZorrosRapier), typeof(MaskOfZorro) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ZorrosRapier), typeof(MaskOfZorro) }; }
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

            c.DropItem(new PowerScroll(SkillName.Fencing, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ZorrosRapier());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaskOfZorro());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Flurry(defender); break;
                    case 1: Parry(); break;
                    case 2: SignatureSlash(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Flurry(Mobile defender)
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(1))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(20, 30);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x37B9, 10, 30, 5052, EffectLayer.Head);
                m.PlaySound(0x3B8);
            }
        }

        public void Parry()
        {
            this.VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(10), delegate { this.VirtualArmor -= 20; });

            this.FixedParticles(0x37B9, 10, 30, 5052, EffectLayer.Waist);
            this.PlaySound(0x3B6);
        }

        public void SignatureSlash(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x37B9, 10, 30, 5052, EffectLayer.Head);
                defender.PlaySound(0x3B7);

                defender.SendMessage("You have been marked by Zorro!");
                defender.Damage(Utility.RandomMinMax(40, 60), this);
                defender.VirtualArmor -= 10;

                Timer.DelayCall(TimeSpan.FromSeconds(30), delegate { defender.VirtualArmor += 10; });
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

    public class ZorrosRapier : BaseSword
    {
        [Constructable]
        public ZorrosRapier()
            : base(0x13B9)
        {
            Name = "Zorro's Rapier";
            Hue = 0x47E;
            WeaponAttributes.HitLeechStam = 50;
            WeaponAttributes.HitLowerDefend = 50;
            WeaponAttributes.UseBestSkill = 1;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 50;
        }

        public ZorrosRapier(Serial serial)
            : base(serial)
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

    public class MaskOfZorro : BaseClothing
    {
        [Constructable]
        public MaskOfZorro()
            : base(0x154B)
        {
            Name = "Mask of Zorro";
            Hue = 0x47E;
            Attributes.BonusDex = 10;
            Attributes.BonusStam = 20;
        }

        public MaskOfZorro(Serial serial)
            : base(serial)
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
