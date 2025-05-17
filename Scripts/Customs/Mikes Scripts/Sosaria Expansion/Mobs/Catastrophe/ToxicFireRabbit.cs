using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a toxic rabbit corpse")]
    public class ToxicFireRabbit : BaseCreature
    {
        // Timers controlling when special abilities occur.
        private DateTime m_NextToxicAura;
        private DateTime m_NextAbilityDelay;

        [Constructable]
        public ToxicFireRabbit() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "Toxic Fire Rabbit";
            // Using the base body and sound from the standard Fire Rabbit
            Body = 0x1FD; // same body as the original fire rabbit (adjust if needed)
            BaseSoundID = 0x188; // typical bunny sound

            // Set a unique toxic hue
            Hue = 0x66A; // a custom hue that suggests toxic energy

            // Stats – boosted to create an advanced threat.
            SetStr(300);
            SetDex(500);
            SetInt(400);

            SetHits(4000);
            SetStam(2000);
            SetMana(2000);

            // Damage output: blended fire and poison damage.
            SetDamage(20, 35);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.MagicResist, 100);
            SetSkill(SkillName.Tactics, 90);
            SetSkill(SkillName.Wrestling, 100);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 50;

            Tamable = false; // advanced monster is not tamable

            // Initialize ability timers.
            m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextAbilityDelay = DateTime.UtcNow;
        }

        public ToxicFireRabbit(Serial serial) : base(serial)
        {
        }

        public override bool BardImmune { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 5);
        }



        // This method is called during combat actions.
        public override void OnActionCombat()
        {
            // Ensure that Combatant is a Mobile before accessing Mobile-specific properties.
            if (!(Combatant is Mobile target) || target.Deleted || target.Map != Map || !InRange(target, 20) || !CanBeHarmful(target) || !InLOS(target))
                return;

            // Use a toxic explosion ability if the delay has elapsed.
            if (DateTime.UtcNow >= m_NextAbilityDelay)
            {
                ToxicExplosion(target);
                m_NextAbilityDelay = DateTime.UtcNow + TimeSpan.FromSeconds(8);
            }
        }

        // This method is called periodically; here we use it to trigger our toxic aura.
        public override void OnThink()
        {
            base.OnThink();

            // Ensure our Combatant is a Mobile before proceeding.
            if (Combatant is Mobile target && !target.Deleted && DateTime.UtcNow >= m_NextToxicAura)
            {
                DoToxicAura(target);
                m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        // Performs a toxic explosion on the given Mobile target.
        public void ToxicExplosion(Mobile target)
        {
            // Mark target as harmful.
            DoHarmful(target);

            Point3D loc = target.Location;

            // Display multiple visual effects using a mix of fire and toxic hues.
            Effects.SendLocationEffect(new Point3D(loc.X + 2, loc.Y, loc.Z), Map, 0x3728, 30, 10, Hue, 0);
            Effects.SendLocationEffect(new Point3D(loc.X - 2, loc.Y, loc.Z), Map, 0x3728, 30, 10, Hue, 0);
            Effects.PlaySound(loc, Map, 0x208);

            // Deal damage: mix of physical, fire, and poison.
            int damage = Utility.RandomMinMax(150, 200);
            AOS.Damage(target, this, damage, 25, 25, 0, 50, 0); // 25% physical, 25% fire, 50% poison damage
        }

        // Creates a toxic aura effect that damages nearby hostile mobiles.
        public void DoToxicAura(Mobile target)
        {
            // Gather all mobiles within a range of 5.
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in eable)
            {
                if (m == this || !CanBeHarmful(m))
                    continue;
                targets.Add(m);
            }
            eable.Free();

            // Display a pulsating effect at the Toxic Fire Rabbit's location.
            Effects.SendLocationEffect(Location, Map, 0x372A, 20, 10, Hue, 0);

            // Apply a small amount of damage (mostly poison) to each affected mobile.
            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(10, 20);
                AOS.Damage(m, this, damage, 0, 25, 0, 75, 0); // 75% poison component
            }
        }

        // When the Toxic Fire Rabbit is damaged by a spell, there is a chance to spawn a toxic spore minion.
        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == null || caster == this)
                return;

            // 25% chance to spawn a minion
            if (Utility.RandomDouble() < 0.25)
                SpawnToxicSporeMinion(caster);
        }

        // Spawns a custom Toxic Spore minion that aids the Toxic Fire Rabbit.
        public void SpawnToxicSporeMinion(Mobile caster)
        {
            Map map = Map;
            if (map == null)
                return;

            BaseCreature minion = new ToxicSporeMinion();
            minion.Name = "Toxic Spore";
            minion.DamageMin = 30;
            minion.DamageMax = 50;
            minion.HitsMaxSeed = Utility.RandomMinMax(500, 600);
            minion.Hits = minion.HitsMaxSeed;
            Team = this.Team; // align teams

            bool validLocation = false;
            Point3D loc = this.Location;
            for (int i = 0; !validLocation && i < 10; i++)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);
                if (map.CanFit(x, y, Z, 16, false, false))
                {
                    loc = new Point3D(x, y, Z);
                    validLocation = true;
                }
                else if (map.CanFit(x, y, z, 16, false, false))
                {
                    loc = new Point3D(x, y, z);
                    validLocation = true;
                }
            }
            if (validLocation)
            {
                minion.MoveToWorld(loc, map);
                minion.Combatant = caster;
            }
        }

        // When the Toxic Fire Rabbit hits a target in melee, it has a chance to apply a toxic debuff.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            if (defender is Mobile target && Utility.RandomDouble() < 0.2)
            {
                target.SendMessage("You feel a burning, toxic sting!");
                // Here we apply extra damage as a stand-in for a damage-over-time effect.
                AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0);
            }
        }

        // Reflects damage back to attacking creatures if they are pets.
        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                if (Utility.RandomDouble() < 0.7)
                    from.Damage(damage, this);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // A small minion class spawned by Toxic Fire Rabbit
    public class ToxicSporeMinion : BaseCreature
    {
        [Constructable]
        public ToxicSporeMinion() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Body = 0x1FD;
            Hue = 0x66A;
            BaseSoundID = 0x188;

            Name = "a toxic spore";
            SetStr(100);
            SetDex(100);
            SetInt(50);

            SetHits(600);
            SetStam(300);
            SetMana(300);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 60);
            SetSkill(SkillName.Tactics, 50);
            SetSkill(SkillName.Wrestling, 60);

            Fame = 3000;
            Karma = -3000;
            VirtualArmor = 30;

            Tamable = false;
        }

        public ToxicSporeMinion(Serial serial) : base(serial)
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
