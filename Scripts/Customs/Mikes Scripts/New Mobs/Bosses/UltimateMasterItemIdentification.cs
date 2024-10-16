using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Indiana Jones")]
    public class UltimateMasterItemIdentification : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterItemIdentification()
            : base(AIType.AI_Melee)
        {
            Name = "Indiana Jones";
            Title = "The Ultimate Archaeologist";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(15000);
            SetMana(1000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Healing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.ItemID, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 75;

            AddItem(new Boots());
            AddItem(new LeatherGloves());
            AddItem(new LeatherCap());
            AddItem(new Shirt());
            AddItem(new LongPants());
            AddItem(new BodySash());

            HairItemID = 0x203C; // Short Hair
            HairHue = 0x47E;
        }

        public UltimateMasterItemIdentification(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ArchaeologistsHat), typeof(WhipOfDiscovery) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(AncientRelic), typeof(TreasureMap) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(GoldenIdol), typeof(AncientVase) }; }
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

            c.DropItem(new PowerScroll(SkillName.ItemID, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ArchaeologistsHat());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new WhipOfDiscovery());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: IdentifyWeakness(defender); break;
                    case 1: ArtifactHunt(); break;
                    case 2: WhipLash(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void IdentifyWeakness(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendMessage("Indiana Jones identifies your weaknesses!");
                defender.PlaySound(0x1F1);
                defender.Damage(Utility.RandomMinMax(10, 20), this);
            }
        }

        public void ArtifactHunt()
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

                // Summon a random treasure near the player
                Item treasure = new Gold(Utility.RandomMinMax(50, 100));
                treasure.MoveToWorld(m.Location, m.Map);

                m.SendMessage("Indiana Jones has summoned a treasure!");
                m.PlaySound(0x2E6);
            }
        }

        public void WhipLash(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendMessage("Indiana Jones lashes at you with his whip, disarming and stunning you!");
                defender.PlaySound(0x48D);
                defender.Paralyze(TimeSpan.FromSeconds(5.0));
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
