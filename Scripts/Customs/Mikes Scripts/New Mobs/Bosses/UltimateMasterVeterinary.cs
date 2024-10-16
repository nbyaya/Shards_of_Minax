using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of James Herriot")]
    public class UltimateMasterVeterinary : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterVeterinary()
            : base(AIType.AI_Mage)
        {
            Name = "James Herriot";
            Title = "The Compassionate Veterinarian";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Veterinary, 120.0);
            SetSkill(SkillName.AnimalLore, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 70;
			
			AddItem(new FancyShirt(Utility.RandomBlueHue()));
            AddItem(new LongPants(Utility.RandomRedHue()));
            AddItem(new Cloak(Utility.RandomRedHue()));
            AddItem(new Boots());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterVeterinary(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(HealersKit), typeof(AnimalCompanion) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(HealersKit), typeof(PetRevivePotion) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(HealersKit), typeof(PetCollar) }; }
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

            c.DropItem(new PowerScroll(SkillName.Veterinary, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new HealersKit());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new AnimalCompanion());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: HealPet(); break;
                    case 1: Revitalize(); break;
                    case 2: PetCommand(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void HealPet()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m is BaseCreature && ((BaseCreature)m).Controlled && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.Hits += Utility.RandomMinMax(50, 70);
                m.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                m.PlaySound(0x1F2);
            }
        }

        public void Revitalize()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m is BaseCreature && ((BaseCreature)m).Controlled && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.Hits += Utility.RandomMinMax(30, 50);
                m.Stam += Utility.RandomMinMax(30, 50);
                m.Mana += Utility.RandomMinMax(30, 50);
                m.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Head);
                m.PlaySound(0x1F4);
            }
        }

        public void PetCommand(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                defender.PlaySound(0x1F2);

                defender.Paralyze(TimeSpan.FromSeconds(6.0));
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
}
