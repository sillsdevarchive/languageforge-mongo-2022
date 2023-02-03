<script lang="ts">
    import { ProjectApi } from '$lib/gen/dotnet';

    const api = new ProjectApi();
    const projectsLoading = api.getAllProjects();
</script>

<div>
    {#await projectsLoading}
        <div>loading...</div>
    {:then projects}
        <div class="projects">
            {#each projects as project}
                <div>
                    {project.projectName} - User Count: {project?.users?.length ?? 0}
                </div>
            {/each}
        </div>
    {:catch error}
        <div>error</div>
    {/await}
</div>

<style>
    .projects {
        display               : grid;
        grid-template-columns : 1fr;
        grid-gap              : 1rem;
    }
</style>