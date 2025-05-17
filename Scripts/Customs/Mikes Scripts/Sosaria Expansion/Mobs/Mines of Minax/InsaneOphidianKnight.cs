using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an insane ophidian knight corpse")]
    public class InsaneOphidianKnight : BaseCreature
    {
        private DateTime m_NextVenomAura;
        private DateTime m_NextSerpentSummon;
        private DateTime m_NextRush;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1360; // Toxic green-purple

        [Constructable]
        public InsaneOphidianKnight()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.5)
        {
            Name = "an Insane Ophidian Knight";
            Body = 86;
            BaseSoundID = 634;
            Hue = UniqueHue;

            // Stats
            SetStr(650, 800);
            SetDex(200, 250);
            SetInt(150, 200);

            SetHits(2000, 2300);
            SetStam(300, 350);
            SetMana(200, 300);

            SetDamage(25, 30);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.Poisoning, 120.0, 130.0);
            SetSkill(SkillName.Magery, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Cooldowns
            m_NextVenomAura    = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextSerpentSummon = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextRush          = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_LastLocation      = this.Location;

            // Guaranteed loot
            PackItem(new GreaterPoisonPotion());
            PackItem(new BlackPearl(Utility.RandomMinMax(20, 30)));
        }

        // --- Poisonous Trail Aura ---
        // Whenever a mobile comes near, pulse poison damage.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (DateTime.UtcNow < m_NextVenomAura || !Alive || m == this || m.Map != this.Map || !m.InRange(Location, 3))
                return;

            m_NextVenomAura = DateTime.UtcNow + TimeSpan.FromSeconds(12);

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);
                target.SendMessage(0x22, "Toxic spines lash out from the knight!"); 
                AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 100, 0);

                // Create a short‑lived poison pool under target
                var pool = new PoisonTile();
                pool.Hue = UniqueHue;
                pool.MoveToWorld(target.Location, Map);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Leave a slithering poison trail behind
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var tile = new QuicksandTile(); // sticky hazard
                tile.Hue = UniqueHue;
                tile.MoveToWorld(m_LastLocation, Map);
            }
            m_LastLocation = Location;

            // Rush attack: leap at target
            if (DateTime.UtcNow >= m_NextRush && InRange(Combatant.Location, 10))
            {
                m_NextRush = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                SlitheringRush();
            }
            // Summon lesser serpents
            else if (DateTime.UtcNow >= m_NextSerpentSummon && InRange(Combatant.Location, 12))
            {
                m_NextSerpentSummon = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                SummonSerpents();
            }
        }

        // --- Rush Attack: Slithering Leap ---
        private void SlitheringRush()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Sssstrike!*");
            PlaySound(0x639);
            // Teleport close and deal heavy physical + poison
            MoveToWorld(target.Location, Map);
            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(60, 80), 50, 0, 0, 50, 0);
            target.ApplyPoison(this, Poison.Lethal);
        }

        // --- Summon Ability: Call of the Scaled ---
        private void SummonSerpents()
        {
            Say("*Rise, my children!*");
            PlaySound(0x5A1);

            int summonCount = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < summonCount; i++)
            {
                var snake = new GiantSerpent(); // assume Serpent is defined
                snake.Hue = UniqueHue;
                snake.MoveToWorld( new Point3D(X + Utility.RandomMinMax(-2,2), Y + Utility.RandomMinMax(-2,2), Z), Map );
            }
        }

        // --- Death Explosion: Toxic Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*Hssss... you perish with me!*");
                Effects.PlaySound(Location, Map, 0x208);
                Effects.SendLocationParticles( EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 20, 60, 5039);

                // Spawn a ring of poison pools
                for (int i = 0; i < 8; i++)
                {
                    int dx = (int)(Math.Cos(i * (Math.PI / 4)) * 2);
                    int dy = (int)(Math.Sin(i * (Math.PI / 4)) * 2);
                    var pool = new PoisonTile();
                    pool.Hue = UniqueHue;
                    pool.MoveToWorld(new Point3D(X + dx, Y + dy, Z), Map);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot, Immunities & Properties ---
        public override bool BleedImmune       { get { return true; } }
        public override Poison PoisonImmune    { get { return Poison.Lethal; } }
        public override Poison HitPoison       { get { return Poison.Lethal; } }
        public override int TreasureMapLevel   { get { return 5; } }
        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.TerathansAndOphidians; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // Small chance for unique Ophidian relic
            if (Utility.RandomDouble() < 0.005)
                PackItem(new OphidianKnightRelic());
        }

        public InsaneOphidianKnight(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextVenomAura    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8,12));
            m_NextSerpentSummon = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18,22));
            m_NextRush          = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12,18));
            m_LastLocation      = this.Location;
        }
    }
}
